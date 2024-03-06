using GROUP2.Dtos;

namespace GROUP2.Services.GroupInvestment
{
    public interface IInvetmentRepo
    {
        InvestmentDtos GetById(int id);
        IEnumerable<InvestmentDtos> GetAll();
        void Add(InvestmentDtos investmentInterest);
        void Update(InvestmentDtos investmentInterest);
        void Delete(int id);

        public void Invest(int userId, int accountId, int interestId, int investmentInterestId, decimal amount);

    }
}
