using GROUP2.Data;
using GROUP2.Dtos;
using GROUP2.Helper;
using Microsoft.EntityFrameworkCore;

namespace GROUP2.Services.GroupInvestment
{
    public class InvestmentRepository : IInvetmentRepo
    {
        private readonly DataContext _dataContext;

        public InvestmentRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void Add(InvestmentDtos investmentInterest)
        {
            // Implement the Add method based on your data access technology.
            var entity = EntityDtoMapper.MapToEntity(investmentInterest);
            _dataContext.Investments.Add(entity);
            _dataContext.SaveChanges();
        }

        public void Delete(int id)
        {
            // Implement the Delete method based on your data access technology.
            var entity = _dataContext.Investments.Find(id);
            if (entity != null)
            {
                _dataContext.Investments.Remove(entity);
                _dataContext.SaveChanges();
            }
        }

        public IEnumerable<InvestmentDtos> GetAll()
        {
            // Implement the GetAll method based on your data access technology.
            var entities = _dataContext.Investments.ToList();
            return entities.Select(EntityDtoMapper.MapToDto);
        }

        public InvestmentDtos GetById(int id)
        {
            var entity = _dataContext.Investments.Find(id);
            return EntityDtoMapper.MapToDto(entity);
        }

        public void Update(InvestmentDtos investmentInterest)
        {
            // Implement the Update method based on your data access technology.
            var entity = EntityDtoMapper.MapToEntity(investmentInterest);
            _dataContext.Investments.Update(entity);
            _dataContext.SaveChanges();
        }

        public void Invest(int userId, int accountId, int interestId, int investmentInterestId, decimal amount)
        {
            //var interest = _interestRepository.GetAll().FirstOrDefault(i => i.Id == interestId);
            //var investmentInterest = _investmentRepository.GetAll().FirstOrDefault(ii => ii.Id == investmentInterestId);

            //if (interest == null || investmentInterest == null)
            //{
            //    // Handle not found interests
            //    return;
            //}

            //_userAccountRepository.DeductAmount(userId, accountId, amount);
        }
    }
}
