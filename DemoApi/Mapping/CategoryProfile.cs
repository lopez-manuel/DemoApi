using AutoMapper;
using DemoApi.Models;
using DemoApi.Models.Dtos;

namespace DemoApi.Mapping;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<Category, CategoryDto>().ReverseMap();
        CreateMap<Category, CreateCategoryDto>().ReverseMap();
    }
}