using GROUP2.Data;
using GROUP2.Models;
using Microsoft.EntityFrameworkCore;

namespace GROUP2.Services.GroupInvestment
{
    public class UserAccountRepository : IUserAccount
    {
        private readonly DataContext _dataContext;

        public UserAccountRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public Models.User GetUserAccountByUserId(long userId)
        {
            return _dataContext.Users.FirstOrDefault(u => u.Id == userId);
        }

        //public void DeductAmount(int userId, decimal amount)
        //{
        //    var userAccount = _dataContext.Users.FirstOrDefault(u => u.Id == userId);

        //    if (userAccount != null)
        //    {
        //        userAccount.Balance -= amount;
        //        _dataContext.SaveChanges();
        //    }
        //}

        public void DeductAmount(int userId, long accountId, decimal amount)
        {
            var userAccount = _dataContext.Users.FirstOrDefault(u => u.Id == userId);

            if (userAccount != null)
            {
                userAccount.Balance -= amount;
                _dataContext.SaveChanges();
            }
        }
    }
}
