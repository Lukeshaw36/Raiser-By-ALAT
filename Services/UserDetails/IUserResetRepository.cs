using GROUP2.Dtos;

namespace GROUP2.Services.User
{
    public interface IUserResetRepository
    {
        Task<ResendOtp> ResendOtpAsync(ResendOtpDto resendOtpDto);
        Task<ResetPassword> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
        Task<PasswordChange> PasswordChangeAsync(PasswordChangeDto passwordChangeDto);
    }
}
