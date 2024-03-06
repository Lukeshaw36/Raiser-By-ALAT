using GROUP2.Dtos;
using GROUP2.Models;

namespace GROUP2.Helper
{
    public class EntityDtoMapper
    {
        public static InvestmentDtos MapToDto(Investment entity)
        {
            return new InvestmentDtos
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                PriceStock = entity.PriceStock,
                url = entity.url,
                MaximumCapital = entity.MaximumCapital,
                UserId = entity.UserId,
                //Users = entity.Users.Select(user => new UserDto { /* Map user properties */ }).ToList()
            };
        }

        public static Investment MapToEntity(InvestmentDtos dto)
        {
            return new Investment
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description,
                url = dto.url,
                PriceStock = dto.PriceStock,
                MaximumCapital = dto.MaximumCapital,
                UserId = dto.UserId,
                //Users = dto.Users.Select(userDto => new User { /* Map user properties */ }).ToList()
            };
        }

        public static InterestDtos MapToDto(Interest entity)
        {
            if (entity == null)
            {
                // Handle the case where the entity is null.
                // For now, let's return null.
                return null;
            }

            var dto = new InterestDtos
            {
                Id = (int)entity.Id,
                Name = entity.Name,
                CreateAt = entity.CreateAt,
                CreatedBy = "Admin"
            };

            if (entity.InvestmentInterests != null)
            {
                dto.InvestmentInterests = entity.InvestmentInterests.Select(ii => MapToDto(ii)).ToList();
            }
            else
            {
                // Handle the case where InvestmentInterests is null.
                // You might want to set it to an empty list or handle it based on your requirements.
                dto.InvestmentInterests = new List<InvestmentDtos>();
            }

            return dto;
        }

       public static Interest MapToEntity(InterestDtos dto)
        {
            return new Interest
            {
                Id = dto.Id,
                Name = dto.Name,
                CreateAt = dto.CreateAt,
                CreatedBy = "Admin",
                InvestmentInterests = dto.InvestmentInterests?.Select(iiDto => MapToEntity(iiDto)).ToList()
            };
        }
    }
}