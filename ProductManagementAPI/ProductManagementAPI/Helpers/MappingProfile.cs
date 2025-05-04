using AutoMapper;
using ProductManagement.Data.Entities;
using ProductManagementAPI.Models;

namespace ProductManagementAPI.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
        }
    }
}
