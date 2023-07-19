using System.Security.Claims;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Entities;
using IConfigurationProvider = AutoMapper.IConfigurationProvider;

namespace OnlineStore.Controllers;

[ApiController]
[Route("api/v1/user/personal")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = ApplicationRole.UserAndAdmin)]
public class UserPersonalController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfigurationProvider _configurationProvider;
    private readonly IMapper _mapper;

    public UserPersonalController(UserManager<ApplicationUser> userManager, IConfigurationProvider configurationProvider, IMapper mapper)
    {
        _userManager = userManager;
        _configurationProvider = configurationProvider;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetPersonalData()
    {
        var id = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        var userDto = _userManager.Users
            .AsNoTracking()
            .Where(u => u.Id == id)
            .ProjectTo<UserPersonalInfoDto>(_configurationProvider)
            .FirstOrDefault();

        return userDto == null 
            ? NotFound() 
            : Json(userDto);
    }
    
    [HttpPut]
    public async Task<IActionResult> UpdatePersonalData(UserEditPersonalDto dto)
    {
        var user = await _userManager.GetUserAsync(User);

        if (user == null)
            return NotFound();

        _mapper.Map(dto, user);

        var identityResult = await _userManager.UpdateAsync(user);

        if (!identityResult.Succeeded)
        {
            foreach (var error in identityResult.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }

            return ValidationProblem();

        }

        return Ok();

    }
}