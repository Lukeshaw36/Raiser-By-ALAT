using GROUP2.Models;

namespace GROUP2.Services.GroupInvestment
{
    //for investment
 public interface IUserAccount
    {
        Models.User GetUserAccountByUserId(long userId);
        void DeductAmount(int userId, long accountId, decimal amount);
    }
}
