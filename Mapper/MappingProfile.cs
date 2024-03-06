using AutoMapper;
using GROUP2.Dtos;
using GROUP2.Models;

namespace GROUP2.Mapper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            // RegisterMap DTO
            CreateMap<UserDto, User>();
            CreateMap<UpdateUserProfileDto, User>();
        }

    }
}
