using GROUP2.Data;
using GROUP2.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net;
using GROUP2.Helper;
using Microsoft.Extensions.Caching.Memory;

namespace GROUP2.Services.User
{
    public class UserResetRepository : IUserResetRepository
    {
        private readonly DataContext _dataContext;
        private readonly IMemoryCache _cache;
        public UserResetRepository(DataContext dataContext, IMemoryCache cache)
        {
            _dataContext = dataContext;
            _cache = cache;
        }

        //public async Task<ResendOtp> ResendOtpAsync(ResendOtpDto resendOtpDto)
        //{
        //    if (resendOtpDto == null || string.IsNullOrEmpty(resendOtpDto.Email))
        //    {
        //        throw new Exception("Invalid email address");
        //    }

        //    try
        //    {
        //        var user = await _dataContext.Users.SingleOrDefaultAsync(u => u.Email == resendOtpDto.Email);

        //        if (user == null)
        //        {
        //            throw new UnauthorizedAccessException("User Not Found");
        //        }

        //        // Generate and update a new OTP
        //        user.OTP = GenerateOTP();
        //        await _dataContext.SaveChangesAsync();

        //        // Resend OTP via Email asynchronously
        //        await SendOtpEmail(user.Email, user.OTP);

        //        return ResendOtp.Success;
        //    }
        //    catch (Exception ex)
        //    {
        //        HandleException(ex);
        //        return ResendOtp.Failure;
        //    }
        //}
        public async Task<ResendOtp> ResendOtpAsync(ResendOtpDto resendOtpDto)
        {
            if (resendOtpDto == null || string.IsNullOrEmpty(resendOtpDto.Email))
            {
                throw new Exception("Invalid email address");
            }

            try
            {
                // Retrieve the user from cache memory
                var cachedUser = _cache.Get<Models.User>("tempUser");

                if (cachedUser == null || cachedUser.Email != resendOtpDto.Email)
                {
                    throw new Exception("User not found in cache memory.");
                }

                // Resend OTP via Email asynchronously
                await SendOtpEmail(cachedUser.Email, cachedUser.OTP);

                return ResendOtp.Success;
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return ResendOtp.Failure;
            }
        }

        public async Task<ResetPassword> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
        {
            // Similar implementation as ResendOtpAsync, adapting it for password reset
            // ...

            if (resetPasswordDto == null || string.IsNullOrWhiteSpace(resetPasswordDto.Email))
            {
                throw new UnauthorizedAccessException("Invalid email");
            }
            try
            {
                // Check if the email exists in the database
                var userModel = await _dataContext.Users.FirstOrDefaultAsync(u => u.Email == resetPasswordDto.Email);

                if (userModel == null)
                {
                    throw new UnauthorizedAccessException("Email not found");
                }

                // Generate a unique token for password reset
                string resetToken = GenerateOTP();
                // Save the token and its expiration time to the user's record
                userModel.ResetToken = resetToken;
                userModel.ResetTokenExpiration = DateTime.UtcNow.AddHours(1);// Token valid for 1 hour


                string resetUrl = $"https://yourwebsite.com/reset-password/{resetToken}";
                await SendResetPasswordEmail(userModel.Email, resetUrl);

                return ResetPassword.Success;
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return ResetPassword.Failure;
            }
        }

        //public async Task<PasswordChange> PasswordChangeAsync(PasswordChangeDto passwordChangeDto)
        //{
        //    if (passwordChangeDto == null || string.IsNullOrWhiteSpace(passwordChangeDto.Email))
        //    {
        //        throw new UnauthorizedAccessException("Invalid email");
        //    }

        //    try
        //    {
        //        var user = await _dataContext.Users
        //            .FirstOrDefaultAsync(u => u.Email == passwordChangeDto.Email);

        //        if (user == null)
        //        {
        //            throw new UnauthorizedAccessException("User not found");
        //        }

        //        if (passwordChangeDto.NewPassword != passwordChangeDto.ConfirmPassword)
        //        {
        //            throw new UnauthorizedAccessException("New password and confirm password do not match");
        //        }
        //        if (!PasswordValidator.IsStrongPassword(passwordChangeDto.NewPassword))
        //        {
        //            throw new UnauthorizedAccessException("New password is not strong enough");
        //        }

        //        string hashedNewPassword = BCrypt.Net.BCrypt.HashPassword(passwordChangeDto.NewPassword);
        //        user.Password = hashedNewPassword;

        //        _dataContext.Users.Update(user);
        //        await _dataContext.SaveChangesAsync();

        //        return PasswordChange.Success;
        //    }
        //    catch (Exception ex)
        //    {
        //        HandleException(ex);
        //        return PasswordChange.Failure;
        //    }
        //}
        public async Task<PasswordChange> PasswordChangeAsync(PasswordChangeDto passwordChangeDto)
        {
            if (passwordChangeDto == null || string.IsNullOrWhiteSpace(passwordChangeDto.Email))
            {
                throw new UnauthorizedAccessException("Invalid email");
            }

            try
            {
                var user = await _dataContext.Users.FirstOrDefaultAsync(u => u.Email == passwordChangeDto.Email);

                if (user == null)
                {
                    throw new UnauthorizedAccessException("User not found");
                }

                if (passwordChangeDto.NewPassword != passwordChangeDto.ConfirmPassword)
                {
                    throw new UnauthorizedAccessException("New password and confirm password do not match");
                }

                if (!PasswordValidator.IsStrongPassword(passwordChangeDto.NewPassword))
                {
                    return PasswordChange.IsNotStrong;
                }

                string hashedNewPassword = BCrypt.Net.BCrypt.HashPassword(passwordChangeDto.NewPassword);
                user.Password = hashedNewPassword;

                _dataContext.Users.Update(user);
                await _dataContext.SaveChangesAsync();

                return PasswordChange.Success;
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return PasswordChange.Failure;
            }
        }




        // Additional methods like ValidateOtpAsync, SendOtpEmail, GenerateOTP, and HandleException
        // can be added based on your needs.

        private async Task SendOtpEmail(string email, string otp)
        {
            try
            {
                using MailMessage mail = new MailMessage();
                {
                    mail.From = new MailAddress("lukemorolakemi@gmail.com");
                    mail.To.Add(email)
                    ;
                    mail.Subject = "Welcome,You Have Been Registered On RAISER";
                    mail.Body = $"Your OTP for registration is: {otp}";

                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com"))
                    {
                        smtp.Port = 587;
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new NetworkCredential("lukemorolakemi@gmail.com", "qmal qkwg sdxx ukrd");
                        smtp.EnableSsl = true;

                        await smtp.SendMailAsync(mail);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to send OTP: {ex.Message}");
            }
        }

        private string GenerateOTP()
        {
            Random rand = new Random();
            string otp = rand.Next(100000, 999999).ToString();
            return otp;
        }
        public static async Task SendResetPasswordEmail(string email, string resetUrl)
        {
            try
            {
                using MailMessage mail = new MailMessage();
                {
                    mail.From = new MailAddress("lukemorolakemi@gmail.com");
                    mail.To.Add(email);
                    mail.Subject = "PASSWORD RESET LINK";
                    mail.Body = $"Your RESET LINK for PASSWORD RECOVERY is: {resetUrl}";

                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com"))
                    {
                        smtp.Port = 587;
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new NetworkCredential("lukemorolakemi@gmail.com", "qmal qkwg sdxx ukrd");
                        smtp.EnableSsl = true;

                        await smtp.SendMailAsync(mail);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to send OTP: {ex.Message}");
            }
        }

        private void HandleException(Exception ex)
        {
            //    var errorResponse = new AdminHelper.ResponseBody<object>
            //    {
            //        Message = $"Internal Server Error: {ex.Message}",
            //        Code = "500",
            //        Data = null
            //    };

            //    throw new ApplicationException(JsonConvert.SerializeObject(errorResponse));
            //}
        }
    }
    public enum ResendOtp
    {

        Success,
        Failure

    }

    public enum ResetPassword
    {

        Success,
        Failure
    }

    public enum PasswordChange
    {
        Success,
        Failure,
        IsNotStrong
    }
}
