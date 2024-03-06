using GROUP2.Data;
using GROUP2.Dtos;
using GROUP2.Helper;
using Microsoft.EntityFrameworkCore;

namespace GROUP2.Services.GroupInvestment
{
    public class InterestRepository : IInterestRepository
    {
        private readonly DataContext _dataContext;

        public InterestRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void Add(InterestDtos interest)
        {
            var entity = EntityDtoMapper.MapToEntity(interest);
            _dataContext.Interests.Add(entity);
            _dataContext.SaveChanges();
        }

        public void Delete(long id)
        {
            var entity = _dataContext.Interests.Find(id);
            if (entity != null)
            {
                _dataContext.Interests.Remove(entity);
                _dataContext.SaveChanges();
            }
        }
        public IEnumerable<InterestDtos> GetAll()
        {
            var entities = _dataContext.Interests.Include(i => i.InvestmentInterests).ToList();
            return entities.Select(EntityDtoMapper.MapToDto);
        }

        //public IEnumerable<InterestDtos> GetAll()
        //{
        //    var entities = _dataContext.Interests.ToList();
        //    return entities.Select(EntityDtoMapper.MapToDto);
        //}

        public InterestDtos GetById(long id)
        {
            var entity = _dataContext.Interests.Find(id);
            return EntityDtoMapper.MapToDto(entity);
        }

        public void Update(InterestDtos interest)
        {
            var entity = EntityDtoMapper.MapToEntity(interest);
            _dataContext.Interests.Update(entity);
            _dataContext.SaveChanges();
        }

        public void AddInvestmentToInterest(long interestId, InvestmentDtos investmentInterest)
        {
            var interest = _dataContext.Interests.Find(interestId);

            if (interest != null)
            {
                var investmentEntity = EntityDtoMapper.MapToEntity(investmentInterest);

                // Set the InterestId property of the Investment entity
                investmentEntity.InterestId = interestId;

                _dataContext.Investments.Add(investmentEntity);
                _dataContext.SaveChanges();
            }
        }
    }
}
