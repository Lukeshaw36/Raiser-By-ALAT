using GROUP2.Data;
using GROUP2.Helper;
using GROUP2.Models;
using GROUP2.Services.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json.Serialization;
//using static GROUP2.Helper.OTPDeletion;

namespace GROUP2.Services.UserDetails
{
    public class ValidateUserOtpRepository : IValidateUserOTPRepository
    {
        private readonly DataContext _dataContext;
        private readonly IMemoryCache _cache;

        public ValidateUserOtpRepository(DataContext dataContext, IMemoryCache cache)
        {
            _dataContext = dataContext;
            _cache = cache;
        }

     
        public async Task<OTPValidation> ValidateOTPAsync(string otp)
        {
            try
            {
                // Find the corresponding entry in the database using the provided OTP
                var userExists = _cache.TryGetValue("tempUser", out Models.User newUser);

                if (userExists)
                {
                    if (newUser.OTP == otp)
                    {
                        // Create user from cache and save to db
                        _dataContext.Users.Add(newUser);
                        await _dataContext.SaveChangesAsync();

                        //// Attempt to delete OTP
                        //bool otpDeleted = DeleteOTP(otp);
                        //if (!otpDeleted)
                        //{
                        //    // If OTP deletion fails, try again
                        //    otpDeleted = DeleteOTP(otp);

                        //    if (!otpDeleted)
                        //    {
                               
                        //        return new OTPValidation { OTPValidationRes = OTPValidationResult.Failure };
                        //    }
                        //}

                        // Valid OTP
                        return new OTPValidation
                        {
                            OTPValidationRes = OTPValidationResult.Success,
                            UserId = newUser.Id,
                            FirstName = newUser.FirstName,
                            LastName = newUser.LastName,
                            Email = newUser.Email,
                            PhoneNumber = newUser.PhoneNumber
                        };
                    }
                    else
                    {
                        // Invalid OTP
                        return new OTPValidation { OTPValidationRes = OTPValidationResult.InvalidOTP };
                    }
                }
                else
                {
                    // Handle the case where "tempUser" is not found in the cache
                    // For example, log an error or return an appropriate response
                    return new OTPValidation { OTPValidationRes = OTPValidationResult.Failure };
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Console.WriteLine($"An error occurred: {ex.Message}");
                return new OTPValidation { OTPValidationRes = OTPValidationResult.Failure };
            }
        }
    }

    public enum OTPValidationResult
    {
        Success,
        InvalidOTP,
        Failure,
    }

    public class OTPValidation
    {
        public long UserId { set; get; }
        public string FirstName { set; get; }
        public string LastName { set; get; }
        public string Email { set; get; }
        public string PhoneNumber { set; get; }
        [JsonIgnore]
        public OTPValidationResult OTPValidationRes { set; get; }
    }
}

