using GROUP2.Dtos;

namespace GROUP2.Services.GroupInvestment
{
    public interface IInterestRepository
    {
        InterestDtos GetById(long id);
        IEnumerable<InterestDtos> GetAll();
        void Add(InterestDtos interest);
        void Update(InterestDtos interest);
        void Delete(long id);
        public void AddInvestmentToInterest(long interestId, InvestmentDtos investmentInterest);

    }
}
