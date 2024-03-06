using GROUP2.Dtos;

namespace GROUP2.Services.UserDetails
{
    public interface IUserLoginRepository
    {
        Task<string> AuthenticateUserAsync(LoginDto loginUser);
    }
}
