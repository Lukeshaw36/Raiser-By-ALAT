
using GROUP2.Data;
using GROUP2.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using GROUP2.Helper;
using static GROUP2.Helper.AccountNumberGenerator;
using GROUP2.Dtos;

namespace GROUP2.Services.User
{
    public class UserRepsitory : IUserRepository
    {
        private readonly DataContext _dataContext;
        private readonly Random _random;
        private readonly ILogger<UserRepsitory> _logger;
        private readonly IMemoryCache _cache;

        public UserRepsitory(DataContext dataContext, ILogger<UserRepsitory> logger, IMemoryCache cache)
        {
            _dataContext = dataContext;
            _logger = logger;
            _cache = cache;
            _random = new Random();
        }


        public string ErrorMessage { get; private set; }

        public bool UpdateUserWalletBalance(int userId, decimal newBalance)
        {
            var user = _dataContext.Users.FirstOrDefault(u => u.Id == userId);
            if (user != null)
            {
                user.WalletBalance = newBalance;
                _dataContext.SaveChanges();
                return true;
            }
            return false;
        }

        public decimal GetUserWalletBalance(int userId)
        {
            var user = _dataContext.Users.FirstOrDefault(u => u.Id == userId);
            if (user != null)
            {
                return user.WalletBalance;
            }
            return 0;
        }

        //public async Task<RegistrationResult> RegisterUserAsync(Models.User registerUser)
        //{
        //    try
        //    {
        //        if (await _dataContext.Users.AnyAsync(u => u.Email == registerUser.Email))
        //        {
        //            return RegistrationResult.EmailAlreadyExists;
        //        }

        //        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerUser.Password);
        //        registerUser.Password = hashedPassword;

        //        var newUser = new Models.User
        //        {
        //            FirstName = registerUser.FirstName,
        //            LastName = registerUser.LastName,
        //            OTP = GenerateOTP(),
        //            Email = registerUser.Email,
        //            PhoneNumber = registerUser.PhoneNumber,
        //            Password = registerUser.Password,
        //            ConfirmPassword = registerUser.ConfirmPassword
        //        };


        //        // create a temp cache
        //        _cache.Set("tempUser", newUser);

        //        await SendOtpEmail(newUser.Email, newUser.OTP);


        //        _logger.LogInformation($"User registered: {newUser.Email}");

        //        return RegistrationResult.Success;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Registration failed");
        //        return RegistrationResult.Failure;
        //    }
        //}
        public async Task<RegistrationResult> RegisterUserAsync(UserDto registerUser)
        {
            try
            {
                // Check if passwords are strong
                if (!PasswordValidator.ArePasswordsValid(registerUser.Password, registerUser.ConfirmPassword))
                {
                    return RegistrationResult.WeakPassword;
                }
                // Check if passwords match
                if (registerUser.Password != registerUser.ConfirmPassword)
                {
                    return RegistrationResult.PasswordsDoNotMatch;
                }

                if (await _dataContext.Users.AnyAsync(u => u.Email == registerUser.Email))
                {
                    return RegistrationResult.EmailAlreadyExists;
                }

                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerUser.Password);
                registerUser.Password = hashedPassword;


                var newUser = new Models.User
                {
                    FirstName = registerUser.FirstName,
                    LastName = registerUser.LastName,
                    OTP = GenerateOTP(),
                 //   AccountNumber = GenerateAccountNumber(10),
                    Email = registerUser.Email,
                    PhoneNumber = registerUser.PhoneNumber,
                    Password = registerUser.Password,
                    // ConfirmPassword = registerUser.ConfirmPassword
                };
                // create a temp cache
                _cache.Set("tempUser", newUser);

                await SendOtpEmail(newUser.Email, newUser.OTP);

                _logger.LogInformation($"User registered: {newUser.Email}");

                return RegistrationResult.Success;
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Failed to register user due to database update error.");
                return RegistrationResult.Failure;
            }
            catch (SmtpException smtpEx)
            {
                _logger.LogError(smtpEx, "Failed to send OTP email: {ErrorMessage}", smtpEx.Message);
                return RegistrationResult.Failure;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Registration failed for an unknown reason.");
                return RegistrationResult.Failure;
            }
        }

        //public async Task<IActionResult> UpdateUserAsync(UpdateUserProfileDto registerUser)
        //{
        //    try
        //    {
        //        var interestInvest = new List<InvestmentInterest>() { };


        //        if (registerUser.InvestmentInterests.Any())
        //        {
        //            registerUser.InvestmentInterests.ForEach(y =>
        //            {
        //                var intr = _dataContext.Interests.Where(x => x.Id.Equals(y.Id)).FirstOrDefault();
        //                if (intr != null)
        //                {
        //                    interestInvest.Add(new InvestmentInterest
        //                    {
        //                        Name = intr.Name,
        //                        UserId = 1
        //                    });
        //                }

        //            });

        //        }

        //        // create a temp cache
        //        _dataContext.InvestmentInterests.AddRange(interestInvest);
        //        _dataContext.SaveChanges();


        //        return Ok(interestInvest); // Return the added interests
        //    }
        //    catch (DbUpdateException dbEx)
        //    {
        //        _logger.LogError(dbEx, "Failed to update user due to database update error.");
        //        return StatusCode(500, "Failed to update user due to database update error.");
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Failed to update user for an unknown reason.");
        //        return StatusCode(500, "Failed to update user for an unknown reason.");
        //    }  
        //        public async Task<UpdateUserResult> UpdateUserAsync(UpdateUserProfileDto registerUser)
        //{
        //    var result = new UpdateUserResult { Success = false, Interests = new List<InvestmentInterest>(), ErrorMessage = "" };

        //    try
        //    {
        //        var interestInvest = new List<InvestmentInterest>();

        //        if (registerUser.InvestmentInterests.Any())
        //        {
        //            registerUser.InvestmentInterests.ForEach(y =>
        //            {
        //                var intr = _dataContext.Interests.FirstOrDefault(x => x.Id == y.Id);
        //                if (intr != null)
        //                {
        //                    interestInvest.Add(new InvestmentInterest
        //                    {
        //                        Name = intr.Name,
        //                        UserId = 1 // Assuming UserId is hardcoded for demo purpose, replace it with the actual user id
        //                    });
        //                }
        //            });
        //        }

        //        // Add interests to the database
        //        _dataContext.InvestmentInterests.AddRange(interestInvest);
        //        await _dataContext.SaveChangesAsync();

        //        result.Success = true;
        //        result.Interests = interestInvest;
        //    }
        //    catch (DbUpdateException dbEx)
        //    {
        //        _logger.LogError(dbEx, "Failed to update user due to database update error.");
        //        result.ErrorMessage = "Failed to update user due to database update error.";
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Failed to update user for an unknown reason.");
        //        result.ErrorMessage = "Failed to update user for an unknown reason.";
        //    }

        //    return result;
        //        }



        //        public class UpdateUserResult
        //        {
        //            public bool Success { get; set; }
        //            public List<InvestmentInterest> Interests { get; set; }
        //            public string ErrorMessage { get; set; }
        //        }
        public async Task<UpdateUserProfileDto> UpdateUserAsync(int userId, UpdateUserProfileDto updateUserProfileDto)
        {
            try
            {
                var user = await _dataContext.Users.FirstOrDefaultAsync(u => u.Id == userId);

                if (user == null)
                {
                    _logger.LogError("User not found");
                    return null;
                }

                // Map properties from DTO to user entity
                user.Experience = updateUserProfileDto.Experience;
                user.Status = updateUserProfileDto.Status;
                user.Income = updateUserProfileDto.Income;




                //// Map InvestmentInterests
                //user.InvestmentInterests = updateUserProfileDto.InvestmentInterests?
                //    .Select(i => new InvestmentInterest { Name = i.Name, UserId = userId })
                //    .ToList();

                // Update user entity
                _dataContext.Users.Update(user);
                await _dataContext.SaveChangesAsync();

                return new UpdateUserProfileDto
                {
                    UserId = user.Id,
                    Experience = user.Experience,
                    Status = user.Status,
                    Income = user.Income
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update user");
                return null;
            }
        }
        private string GenerateOTP()
        {
            string otp = _random.Next(100000, 999999).ToString();
            return otp;
        }

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
                _logger.LogError(ex, "Failed to send OTP: {ErrorMessage}", ex.Message);
                throw new Exception($"Failed to send OTP: {ex.Message}");
            }
        }
    }

    public enum RegistrationResult
    {
        Success,
        EmailAlreadyExists,
        WeakPassword,
        PasswordsDoNotMatch,
        Failure

    }
}

