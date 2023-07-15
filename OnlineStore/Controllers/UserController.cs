using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Contexts;
using OnlineStore.Entities;

using IConfigurationProvider = AutoMapper.IConfigurationProvider;

namespace OnlineStore.Controllers;

[ApiController]
[Route("api/v1/user")]
public class UserController: Controller
{
    private readonly IConfigurationProvider _provider;
    private readonly OnlineStoreContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public UserController(IConfigurationProvider provider, OnlineStoreContext context, UserManager<ApplicationUser> userManager)
    {
        _provider = provider;
        _context = context;
        _userManager = userManager;
    }

    [HttpPost]
    public async Task<IActionResult> Create()
    {
        var user = new ApplicationUser()
        {
            Email = "asdasdas@gmail.com",
            UserName = "adAdi",
            FirstName = "Banana",
            LastName = "Orange"
        };
        
        var result = await _userManager.CreateAsync(user, "Qwerty@1");

        return Json(user);
    }
}