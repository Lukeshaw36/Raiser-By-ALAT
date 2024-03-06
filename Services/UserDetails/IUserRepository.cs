using GROUP2.Dtos;
using GROUP2.Models;
using Microsoft.AspNetCore.Mvc;
using static GROUP2.Services.User.UserRepsitory;

namespace GROUP2.Services.User
{
    public interface IUserRepository
    {
        Task<RegistrationResult> RegisterUserAsync(UserDto registerUser);

        //Task<UpdateUserResult> UpdateUserAsync(UpdateUserProfileDto registerUser);
        Task<UpdateUserProfileDto> UpdateUserAsync(int userId, UpdateUserProfileDto updateUserProfileDto);
        decimal GetUserWalletBalance(int userId);
        bool UpdateUserWalletBalance(int userId, decimal newBalance);
        string ErrorMessage { get; }
    }
}
