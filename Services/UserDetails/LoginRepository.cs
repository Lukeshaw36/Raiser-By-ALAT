using GROUP2.Data;
using GROUP2.Dtos;
using GROUP2.Helper;
using GROUP2.Services.UserDetails;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Net;
using System.Security.Claims;
using System.Text;
using GROUP2.Models;

namespace GROUP2.Services.UserDetails
{
    public class LoginRepository : IUserLoginRepository
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<LoginRepository> _logger;

        public LoginRepository(DataContext context, IConfiguration configuration, ILogger<LoginRepository> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<string> AuthenticateUserAsync(LoginDto loginuser)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginuser.Email);

            if (user == null)
            {
                return null; // User not found
            }

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(loginuser.Password, user.Password);

            if (!isPasswordValid)
            {
                return null; // Invalid credentials
            }

            // Check if the user has logged in for the first time
            if (!user.HasLoggedIn)
            {
                // Send account number email
               // await SendAccountNumberEmail(user);

                // Update the flag to indicate that the user has logged in for the first time
                user.HasLoggedIn = true;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            }

            // If credentials are valid, create and return a JWT token
            return await CreateToken(user);
        }

        //private async Task SendAccountNumberEmail(Models.User user)
        //{
        //    try
        //    {

        //        string accountNumber = AccountNumberGenerator.GenerateAccountNumber(10); // Generate a 10-digit account number

        //        // Update user's account number in the database
        //        user.AccountNumber = accountNumber;
        //        _context.Users.Update(user);
        //        await _context.SaveChangesAsync();
        //        using MailMessage mail = new MailMessage();
        //        {
        //            mail.From = new MailAddress("lukemorolakemi@gmail.com");
        //            mail.To.Add(user.Email);
        //            mail.Subject = "Your Account Number";
        //            mail.Body = $"Dear user,\n\nYour account number is: {accountNumber}\n\nThank you for choosing RAISER.";

        //            using (SmtpClient smtp = new SmtpClient("smtp.gmail.com"))
        //            {
        //                smtp.Port = 587;
        //                smtp.UseDefaultCredentials = false;
        //                smtp.Credentials = new NetworkCredential("lukemorolakemi@gmail.com", "qmal qkwg sdxx ukrd");
        //                smtp.EnableSsl = true;

        //                await smtp.SendMailAsync(mail);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Failed to send account number: {ErrorMessage}", ex.Message);
        //        throw new Exception($"Failed to send account number: {ex.Message}");
        //    }
        //}

        private async Task<string> CreateToken(Models.User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, "User"),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.GivenName, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName),
                new Claim(ClaimTypes.MobilePhone, user.PhoneNumber)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value!));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
    }
}
