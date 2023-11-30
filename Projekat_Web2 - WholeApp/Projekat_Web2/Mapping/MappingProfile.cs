using AutoMapper;
using Projekat_Web2.DTO;
using Projekat_Web2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projekat_Web2.Mapping
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
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Order, OrderDto>().ReverseMap();
            CreateMap<Order, PickupOrderDto>().ReverseMap();          
        }
    }
}
