using AutoMapper;
using OnlineStore.Controllers;
using OnlineStore.Entities;

namespace OnlineStore.Mappers;

public class UserPersonalProfile: Profile
{
    public UserPersonalProfile()
    {
        CreateProjection<ApplicationUser, UserPersonalInfoDto>();
    }
}