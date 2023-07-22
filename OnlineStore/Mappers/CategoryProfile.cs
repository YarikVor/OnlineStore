using AutoMapper;
using OnlineStore.Controllers;
using OnlineStore.Dtos;
using OnlineStore.Entities;

namespace OnlineStore.Mappers;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateProjection<Category, CategoryInfoDto>();
        CreateProjection<Category, CategoryLinearDto>();
        CreateProjection<Category, CategoryBlockDto>();
        CreateProjection<Category, CategoryHierarchyDto>();

    }
}
