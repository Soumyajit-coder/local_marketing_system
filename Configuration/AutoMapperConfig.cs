using AutoMapper;
using localMarketingSystem.DAL.Entities;
using localMarketingSystem.DTOs;

namespace localMarketingSystem.Configuration
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig() 
        { 
            CreateMap<MUser, UpdateUserDTO>().ReverseMap();
            CreateMap<MUser, UserListDTO>().ReverseMap();
        } 
    }
}
