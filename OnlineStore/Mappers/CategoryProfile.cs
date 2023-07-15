using AutoMapper;
using OnlineStore.Dtos;
using OnlineStore.Entities;

namespace OnlineStore.Mappers;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateProjection<Blog, BlogBlockDto>();
    }
}