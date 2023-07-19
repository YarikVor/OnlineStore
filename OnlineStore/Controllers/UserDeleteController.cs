using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Entities;

namespace OnlineStore.Controllers;

[ApiController]
[Route("api/v1/user")]
public class UserDeleteController: Controller
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserDeleteController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    /// <summary>
    /// Destroy user by token
    /// </summary>
    /// <response code="200"> OK, destroy.</response>
    /// <response code="400"> If errors</response>
    /// <response code="401"> Returns 401 (Unauthorized). Please write refresh-token in a header (httpClient) </response>
    /// <response code="404"> Returns 404 if the user is not found</response>
    /// <response code="404"> Returns 404 if the user is not found</response>
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = ApplicationRole.UserAndAdmin)]
    [HttpDelete]
    public async Task<IActionResult> DeleteSelf()
    {
        var user = await _userManager.GetUserAsync(User);

        if (user == null)
            return NotFound();

        var result = await _userManager.DeleteAsync(user);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors) ModelState.AddModelError(error.Code, error.Description);
            return ValidationProblem();
        }

        return Ok();
    }
}