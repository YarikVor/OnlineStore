using AutoMapper;
using OnlineStore.Controllers;
using OnlineStore.Entities;

namespace OnlineStore.Mappers;

public class BlogProfile : Profile
{
    public BlogProfile()
    {
        CreateProjection<Blog, BlogInfoDto>();
    }
}