using AutoMapper;
using UserAdminAPI.DTO;
using UserAdminAPI.Models;

namespace UserAdminAPI.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, CreateUserDto>().ReverseMap();
            CreateMap<Admin, CreateUserDto>().ReverseMap();
            CreateMap<Consumer, CreateUserDto>().ReverseMap();
            CreateMap<Deliverer, CreateUserDto>().ReverseMap();
            CreateMap<User, DisplayUserDto>().ReverseMap();
            CreateMap<Deliverer, DisplayDelivererDto>().ReverseMap();
        }
    }
}
