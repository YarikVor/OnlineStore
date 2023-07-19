using AutoMapper;
using OnlineStore.Controllers;
using OnlineStore.Dtos;
using OnlineStore.Entities;

namespace OnlineStore.Mappers;

public class UserSignProfile : Profile
{
    public UserSignProfile()
    {
        CreateMap<RegistrationDto, ApplicationUser>();
        
        CreateProjection<ApplicationUser, UserFullInfoDto>()
            .ForMember(
                dto => dto.Role,
                opt => opt.MapFrom(au => au.UserRoles.Select(e => e.Role.Name)));
        
        CreateMap<ApplicationUser, UserFullInfoDto>();
        
        //UserEditPersonalDto -> ApplicationUser
        CreateMap<UserEditPersonalDto, ApplicationUser>()
            .ForMember(
                u => u.NormalizedEmail,
                opt => opt.MapFrom(dto => dto.Email.ToUpper()));
    }
}