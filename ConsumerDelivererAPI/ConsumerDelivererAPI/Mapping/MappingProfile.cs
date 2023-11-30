using AutoMapper;
using ConsumerDelivererAPI.Dto;
using ConsumerDelivererAPI.Models;

namespace ConsumerDelivererAPI.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {           
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Order, OrderDto>().ReverseMap();
            CreateMap<Order, PickupOrderDto>().ReverseMap();
        }
    }
}
