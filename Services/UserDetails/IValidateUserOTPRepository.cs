namespace GROUP2.Services.UserDetails
{
    public interface IValidateUserOTPRepository
    {
        Task<OTPValidation> ValidateOTPAsync(string otp);
        //Task<OTPValidationResult> ValidateOTPAsync(string otp);
    }
}
