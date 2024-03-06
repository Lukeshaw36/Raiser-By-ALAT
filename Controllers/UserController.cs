using AutoMapper;
using GROUP2.Dtos;
using GROUP2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static GROUP2.Helper.CustomLoginResponse;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using static GROUP2.Helper.CustomRegisterResponse;
using GROUP2.Helper;
using GROUP2.Services.User;
using GROUP2.Services.UserDetails;

namespace GROUP2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserLoginRepository _userLoginRepository;
        private readonly IValidateUserOTPRepository _validateOTPRepository;
        private readonly IUserResetRepository _userResetRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UserRepsitory> _logger;

        public UserController(IUserRepository userRepository, IUserLoginRepository userLoginRepository, IValidateUserOTPRepository validateOTPRepository, IUserResetRepository userResetRepository, ILogger<UserRepsitory> logger, IMapper mapper)
        {
            _userRepository = userRepository;
            _userLoginRepository = userLoginRepository;
            _validateOTPRepository = validateOTPRepository;
            _userResetRepository = userResetRepository;
            _mapper = mapper;
            _logger = logger;

        }
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserDto registerUser)
        {
            //try
            //{
            //    var registerMap = _mapper.Map<User>(registerUser);
            //    var registrationResult = await _userRepository.RegisterUserAsync(registerUser);

            //    // Custom response based on the registration result
            //    var response = registrationResult switch
            //    {
            //        RegistrationResult.Success => new CustomNewResponse
            //        {
            //            Message = "Registration process initiated,please validate OTP",
            //            Success = true,

            //            //UserDetails = new UserDetail
            //            //{

            //            //    FirstName = registerUser.FirstName,
            //            //    LastName = registerUser.LastName,
            //            //    Email = registerUser.Email,
            //            //    PhoneNumber = registerUser.PhoneNumber,
            //            //    // You may exclude Password from being returned in the response for security reasons.
            //            //    // Password = registerUser.Password
            //            //}
            //        },
            //        RegistrationResult.EmailAlreadyExists => new CustomNewResponse { Message = "Email already exists", Success = false },
            //        RegistrationResult.Failure => new CustomNewResponse { Message = "Registration failed", Success = false },
            //        RegistrationResult.WeakPassword => new CustomNewResponse { Message = "Password is not strong enough", Success = false },
            //        RegistrationResult.PasswordsDoNotMatch => new CustomNewResponse { Message = "Passwords do not match", Success = false },
            //        _ => new CustomNewResponse { Message = "Unknown registration result", Success = false },
            //    };


            //    return Ok(response);
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError(ex, "Error during user registration");
            //    return StatusCode(500, new CustomNewResponse { Message = "Internal server error", Success = false });
            //}
            try
            {
                var registerMap = _mapper.Map<User>(registerUser);
                var registrationResult = await _userRepository.RegisterUserAsync(registerUser);

                // Custom response based on the registration result
                IActionResult response;

                switch (registrationResult)
                {
                    case RegistrationResult.Success:
                        response = Ok(new CustomNewResponse
                        {
                            Message = "Registration process initiated, please validate OTP",
                            Success = true
                        });
                        break;
                    case RegistrationResult.EmailAlreadyExists:
                        response = BadRequest(new CustomNewResponse
                        {
                            Message = "Email already exists",
                            Success = false
                        });
                        break;
                    case RegistrationResult.Failure:
                        response = StatusCode(500, new CustomNewResponse
                        {
                            Message = "Registration failed",
                            Success = false
                        });
                        break;
                    case RegistrationResult.WeakPassword:
                        response = BadRequest(new CustomNewResponse
                        {
                            Message = "Password is not strong enough",
                            Success = false
                        });
                        break;
                    case RegistrationResult.PasswordsDoNotMatch:
                        response = BadRequest(new CustomNewResponse
                        {
                            Message = "Passwords do not match",
                            Success = false
                        });
                        break;
                    default:
                        response = StatusCode(500, new CustomNewResponse
                        {
                            Message = "Unknown registration result",
                            Success = false
                        });
                        break;
                }

                return response;
            }
            catch (Exception ex)
            {
                // Handle exceptions here
                return StatusCode(500, new CustomNewResponse
                {
                    Message = "An error occurred during registration",
                    Success = false
                });
            }
        }

        //[HttpPost("Login")]
        //public async Task<IActionResult> Login(LoginDto login)
        //{
        //    // Check if the provided DTO is valid (e.g., required fields are not null)
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest("Invalid input");
        //    }

        //    // Authenticate the user using the injected service
        //    string token = await _userLoginRepository.AuthenticateUserAsync(login);

        //    if (token == null)
        //           {
        //        var invalidtoken = new LoginResponse
        //        {
        //            Message = "Invalid Credentials!",
        //            Token = "Invalid Token",


        //        };
        //         return Unauthorized(invalidtoken);
        //    }

        //    // Retrieve user details from the token and create a UserDetail object
        //    var userClaims = new JwtSecurityTokenHandler().ReadToken(token) as JwtSecurityToken;
        //    var userDetail = new Detail
        //    {
        //        FirstName = userClaims?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value,
        //        LastName = userClaims?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value,
        //        Email = userClaims?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value,
        //        PhoneNumber = userClaims?.Claims.FirstOrDefault(c => c.Type ==ClaimTypes.MobilePhone)?.Value
        //        // You may exclude Password from being returned in the response for security reasons.
        //        // Password = userClaims?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Password)?.Value
        //    };

        //    // Create the custom response object
        //    var response = new LoginResponse
        //    {
        //        Token = token,
        //        UserDetails = userDetail
        //    };

        //    // If credentials are valid, return the JWT token and user details
        //    return Ok(response);
        //}
        //[HttpPost("Login")]
        //public async Task<IActionResult> Login(LoginDto login)
        //{
        //    // Check if the provided DTO is valid (e.g., required fields are not null)
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest("Invalid input");
        //    }

        //    // Authenticate the user using the injected service
        //    string token = await _userLoginRepository.AuthenticateUserAsync(login);

        //    if (token == null)
        //    {
        //        var invalidTokenResponse = new InvalidTokenResponse
        //        {
        //            Message = "Invalid Credentials!",
        //            Success = false 

        //        };

        //        return Unauthorized(invalidTokenResponse);
        //    }

        //    // Retrieve user details from the token and create a UserDetail object
        //    var userClaims = new JwtSecurityTokenHandler().ReadToken(token) as JwtSecurityToken;
        //    var userDetail = new Detail
        //    {
        //         UserId = userClaims?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value,
        //        FirstName = userClaims?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value,
        //        LastName = userClaims?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value,
        //        Email = userClaims?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value,
        //        PhoneNumber = userClaims?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.MobilePhone)?.Value
        //        // You may exclude Password from being returned in the response for security reasons.
        //        // Password = userClaims?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Password)?.Value
        //    };

        //    // Create the custom response object
        //    var response = new LoginResponse
        //    {
        //        UserId = 
        //        Message = "Login Successful",
        //        Success = true, 
        //        Token = token,
        //        UserDetails = userDetail
        //    };

        // If credentials are valid, return the JWT token and user details
        // return Ok(response);
        // }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto login)
        {
            // Check if the provided DTO is valid (e.g., required fields are not null)
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid input");
            }

            // Authenticate the user using the injected service
            string token = await _userLoginRepository.AuthenticateUserAsync(login);

            if (token == null)
            {
                var invalidTokenResponse = new InvalidTokenResponse
                {
                    Message = "Invalid Credentials!",
                    Success = false
                };

                return Unauthorized(invalidTokenResponse);
            }

            // Retrieve user details from the token and create a UserDetail object
            var userClaims = new JwtSecurityTokenHandler().ReadToken(token) as JwtSecurityToken;

            // Initialize userId variable to store the parsed user ID
            int userId;

            // Try parsing the user ID string to an integer
            if (!string.IsNullOrEmpty(userClaims?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value) &&
                int.TryParse(userClaims?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value, out userId))
            {
                // userId is successfully parsed from the string, you can use it as an int
            }
            else
            {
                // userIdString is either null, empty, or not a valid integer string
                // Handle the case where the user ID cannot be parsed to an int

                // For example, you can set userId to a default value or handle it based on your application's logic
                userId = -1; // Set a default value, or handle it based on your specific requirements
            }

            var userDetail = new Detail
            {
                UserId = userId,
                FirstName = userClaims?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value,
                LastName = userClaims?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value,
                Email = userClaims?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value,
                PhoneNumber = userClaims?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.MobilePhone)?.Value
               
            };

            // Create the custom response object
            var response = new LoginResponse
            {
                UserId = userDetail.UserId, // Include the parsed user ID in the response
                Message = "Login Successful",
                Success = true,
                Token = token,
                UserDetails = userDetail
            };

            return Ok(response);
        }
        //[HttpPost("validate")]
        //public async Task<IActionResult> ValidateOTP(ValidateOtpDto validateOTP)
        //{
        //    if (validateOTP == null)
        //    {
        //        return BadRequest("Invalid OTP data");
        //    }

        //    // Use the injected service to validate the OTP
        //    var result = await _validateOTPRepository.ValidateOTPAsync(validateOTP.OTP);



        //    // Custom response based on the OTP validation result

        //    var response = result.OTPValidationRes switch
        //    {
        //        OTPValidationResult.Success => new Response<OTPValidation>
        //        {
        //            Message = "OTP validated successfully!",
        //            Success = true,
        //            Code = "00",
        //            Data = result
        //        },
        //        OTPValidationResult.InvalidOTP => new Response<OTPValidation>
        //        {
        //            Message = "Invalid OTP or user not found",
        //            Success = false,
        //            Code = "01"
        //        },
        //        OTPValidationResult.Failure => new Response<OTPValidation>
        //        {
        //            Message = "An error occurred while validating OTP",
        //            Success = false,
        //            Code = "01"
        //        },
        //        _ => new Response<OTPValidation>
        //        {
        //            Message = "Unknown OTP validation result",
        //            Success = false,
        //            Code = "01"
        //        }
        //    };

        //    return Ok(response);




        //}
        [HttpPost("validate")]
        public async Task<IActionResult> ValidateOTP(ValidateOtpDto validateOTP)
        {
            if (validateOTP == null)
            {
                return BadRequest("Invalid OTP data");
            }

            // Use the injected service to validate the OTP
            var result = await _validateOTPRepository.ValidateOTPAsync(validateOTP.OTP);

            // Custom response based on the OTP validation result
            IActionResult response;

            switch (result.OTPValidationRes)
            {
                case OTPValidationResult.Success:
                    response = Ok(new Response<OTPValidation>
                    {
                        Message = "OTP validated successfully!",
                        Success = true,
                        Code = "00",
                        Data = result
                    });
                    break;
                case OTPValidationResult.InvalidOTP:
                    response = BadRequest(new Response<OTPValidation>
                    {
                        Message = "Invalid OTP or user not found",
                        Success = false,
                        Code = "01"
                    });
                    break;
                case OTPValidationResult.Failure:
                    response = StatusCode(500, new Response<OTPValidation>
                    {
                        Message = "An error occurred while validating OTP",
                        Success = false,
                        Code = "01"
                    });
                    break;
                default:
                    response = StatusCode(500, new Response<OTPValidation>
                    {
                        Message = "Unknown OTP validation result",
                        Success = false,
                        Code = "01"
                    });
                    break;
            }

            return response;
        }

        //[HttpPost("resend-otp")]
        //public async Task<IActionResult> ResendNewOtp([FromBody] ResendOtpDto resendOtpDto)
        //{
        //    if (resendOtpDto == null)
        //    {
        //        return BadRequest("Invalid OTP data");
        //    }
        //    // Use the injected service to resend the OTP
        //    var result = await _userResetRepository.ResendOtpAsync(resendOtpDto);
        //    // Custom response based on the OTP resend result
        //    var response = result switch
        //    {
        //        ResendOtp.Success => new CustomResetResponse
        //        {
        //            Message = "OTP resent successfully!",
        //            Success = true
        //        },
        //        ResendOtp.Failure => new CustomResetResponse
        //        {
        //            Message = "An error occurred while sending OTP",
        //            Success = false
        //        },
        //        _ => new CustomResetResponse
        //        {
        //            Message = "Unknown OTP result",
        //            Success = false
        //        }
        //    };

        //    return Ok(response);


        //}
        [HttpPost("resend-otp")]
        public async Task<IActionResult> ResendNewOtp([FromBody] ResendOtpDto resendOtpDto)
        {
            if (resendOtpDto == null)
            {
                return BadRequest("Invalid OTP data");
            }

            // Use the injected service to resend the OTP
            var result = await _userResetRepository.ResendOtpAsync(resendOtpDto);

            // Custom response based on the OTP resend result
            IActionResult response;

            switch (result)
            {
                case ResendOtp.Success:
                    response = Ok(new CustomResetResponse
                    {
                        Message = "OTP resent successfully!",
                        Success = true
                    });
                    break;
                case ResendOtp.Failure:
                    response = StatusCode(500, new CustomResetResponse
                    {
                        Message = "An error occurred while sending OTP",
                        Success = false
                    });
                    break;
                default:
                    response = StatusCode(500, new CustomResetResponse
                    {
                        Message = "Unknown OTP result",
                        Success = false
                    });
                    break;
            }

            return response;
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetNewPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            var result = await _userResetRepository.ResetPasswordAsync(resetPasswordDto);

            switch (result)
            {
                case ResetPassword.Success:
                    return Ok(new { Message = "Password reset email sent successfully. Check your email for further instructions." });
                case ResetPassword.Failure:
                    return BadRequest(new { Message = "Failed to initiate password reset. Please check your email address and try again." });
                default:
                    return StatusCode(500, new { Message = "Internal Server Error." });
            }
        }

        [HttpPost("change-password")]
      
      
            public async Task<IActionResult> PasswordChangeAsync([FromBody] PasswordChangeDto passwordChangeDto)
            {
                try
                {
                    var result = await _userResetRepository.PasswordChangeAsync(passwordChangeDto);

                    return result switch
                    {
                        PasswordChange.Success => Ok(new { Message = "Password successfully changed." }),
                        PasswordChange.Failure => BadRequest(new { Message = "Failed to change password. Passwords do not match." }),
                        PasswordChange.IsNotStrong => BadRequest(new { Message = "New password is not strong enough" }),
                        _ => BadRequest("Unexpected result from password change operation")
                    };
                }
                catch (UnauthorizedAccessException ex)
                {
                    return Unauthorized(ex.Message);
                }
                catch (Exception ex)
                {
                    // Log or handle unexpected exceptions
                    return StatusCode(500, "Internal Server Error");
                }
            }

        //[HttpPost("update")]
        //public async Task<IActionResult> UpdateUserAsync(UpdateUserProfileDto registerUser)
        //{
        //    var map= _mapper.Map<User>(registerUser);
        //    var updateResult = await _userRepository.UpdateUserAsync(registerUser);

        //    if (updateResult.Success)
        //    {
        //        return Ok(updateResult.Interests);
        //    }
        //    else
        //    {
        //        return StatusCode(500, updateResult.ErrorMessage);
        //    }
        //}
        [HttpPost("{userId}")]
        public async Task<IActionResult> UpdateUser(int userId, UpdateUserProfileDto updateUserProfileDto)
        {
            //var success = await _userRepository.UpdateUserAsync(userId, updateUserProfileDto);

            //return Ok(success);

            //if (success)
            //{
            //    return Ok(success);
            //}
            //else
            //{
            //    return NotFound("User not found or failed to update user");
           
            
                var userProfile = await _userRepository.UpdateUserAsync(userId, updateUserProfileDto);

                if (userProfile != null)
                {
                    return Ok(userProfile);
                }
                else
                {
                    return NotFound("User not found or failed to update user");
                }
                //}
            }

       
        [HttpPost("{userId}/wallet-balance")]
        public IActionResult UpdateUserWalletBalance(int userId, [FromBody] decimal newBalance)
        {
            var success = _userRepository.UpdateUserWalletBalance(userId, newBalance);
            if (!success)
            {
                return NotFound(new Response<object>
                {
                    Message = "User not found",
                    Success = false,
                    Code = "404"
                });
            }
            // Fetch the updated wallet balance after the update
            var updatedBalance = _userRepository.GetUserWalletBalance(userId);

            return Ok(new Response<decimal>
            {
                Message = "User wallet balance updated successfully",
                Success = true,
                Code = "200",
                Data = updatedBalance
            });

        }

        [HttpGet("users/{userId}/wallet-balance")]
        public IActionResult GetUserWalletBalance(int userId)
        {
            var balance = _userRepository.GetUserWalletBalance(userId);
            if (balance == 0)
            {
                return NotFound(new Response<object>
                {
                    Message = "User not found or wallet balance is zero",
                    Success = false,
                    Code = "404"
                });
            }
            return Ok(new Response<decimal>
            {
                Message = "User wallet balance retrieved successfully",
                Success = true,
                Code = "200",
                Data = balance
            });
        }



    }



}


