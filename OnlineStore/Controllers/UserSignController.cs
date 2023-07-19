using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Entities;

namespace OnlineStore.Controllers;

[ApiController]
[Route("api/v1/user")]
public class UserSignController : Controller
{
    private readonly IMapper _mapper;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ITokenGenerator _tokenGenerator;
    private readonly UserManager<ApplicationUser> _userManager;

    public UserSignController(UserManager<ApplicationUser> userManager, IMapper mapper,
        SignInManager<ApplicationUser> signInManager, ITokenGenerator tokenGenerator,
        RoleManager<ApplicationRole> roleManager)
    {
        _userManager = userManager;
        _mapper = mapper;
        _signInManager = signInManager;
        _tokenGenerator = tokenGenerator;
        _roleManager = roleManager;
    }


    /// <summary>
    ///     Registration a user
    /// </summary>
    /// <param name="dto">The form for registration </param>
    /// <returns><see cref="UserFullInfoDto" />, access- and refresh- token</returns>
    /// <response code="200"> Returns 200 and returns userInfo.</response>
    /// <response code="400"> Returns 400 if the query is invalid (not create the user, add a default role). </response>
    [HttpPost]
    public async Task<IActionResult> Registration(RegistrationDto dto)
    {
        var user = _mapper.Map<ApplicationUser>(dto);
        user.UserName = $"user{Guid.NewGuid():N}";

        var userCreateResult = await _userManager.CreateAsync(user, dto.Password);

        if (!userCreateResult.Succeeded)
        {
            AddErrorsInModelState(userCreateResult);
            return ValidationProblem(ModelState);
        }

        var roleAddResult = await _userManager.AddToRoleAsync(user, ApplicationRole.User);

        if (!roleAddResult.Succeeded)
        {
            AddErrorsInModelState(userCreateResult);
            return ValidationProblem(ModelState);
        }

        var token = GetToken(user, ApplicationRole.User);
        var userResult = _mapper.Map<UserFullInfoDto>(user);
        userResult.Role = ApplicationRole.User;

        var response = new UserTokenDto
        {
            User = userResult,
            AccessToken = token.Access,
            RefreshToken = token.Refresh
        };

        return Json(response);
    }

    /// <summary>
    ///     Login a user
    /// </summary>
    /// <param name="dto">The form for login </param>
    /// <returns><see cref="UserFullInfoDto" />, access- and refresh- token</returns>
    /// <response code="200"> Returns 200 and returns userInfo.</response>
    /// <response code="400"> Returns 400 if the query is invalid (not available the user or invalid password). </response>
    /// <response code="404"> Returns 404 if the user is not found or the user hasn't any role. </response>
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var user = await _userManager.Users
            .FirstOrDefaultAsync(u => u.Email == dto.Email);

        if (user == null)
        {
            ModelState.AddModelError("userNotFound", "User is not avaible");
            return NotFound(ModelState);
        }

        var signInResult = await _signInManager.PasswordSignInAsync(user, dto.Password, false, false);

        if (!signInResult.Succeeded)
        {
            ModelState.AddModelError("passwordIsNotValid", "Password is not valid");
            return ValidationProblem(ModelState);
        }

        var roleName = (await _userManager.GetRolesAsync(user)).FirstOrDefault();

        if (roleName == null) return NotFound("User doesn't have roles");

        var userDto = _mapper.Map<UserFullInfoDto>(user);
        userDto.Role = roleName;

        var token = GetToken(userDto);
        var result = new UserTokenDto
        {
            User = userDto,
            AccessToken = token.Access,
            RefreshToken = token.Refresh
        };

        return Json(result);
    }

    /// <summary>
    ///     Get access-token by refresh-token
    ///     <code>Authorization: Bearer [refresh-token]</code>
    /// </summary>
    /// <returns><see cref="UserFullInfoDto" /> and access-token</returns>
    /// <response code="200"> OK, returns json-object.</response>
    /// <response code="401"> Returns 401 (Unauthorized). Please write refresh-token in a header (httpClient) </response>
    /// <response code="404"> Returns 404 if the user is not found or the user hasn't any role. </response>
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "refresh")]
    [HttpGet("access-token")]
    public async Task<IActionResult> GetAccessToken()
    {
        var user = await _userManager.GetUserAsync(User);

        if (user == null)
            return NotFound();

        var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();

        if (role == null)
            return NotFound();

        var token = GetAccessToken(user.Id, role);
        var userDto = _mapper.Map<UserFullInfoDto>(user);
        userDto.Role = role;

        var result = new UserAccessTokenDto
        {
            AccessToken = token,
            User = userDto
        };

        return Json(result);
    }

    [HttpPost("password/send-recovery")]
    public async Task<IActionResult> RecoveryPassword(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user == null) return NotFound();

        var code = await _userManager.GeneratePasswordResetTokenAsync(user);

        //TODO: send to mail

        return Ok(code);
    }

    [HttpPost("password/reset")]
    public async Task<IActionResult> ResetPassword(RecoveryDto dto)
    {
        var user = await _userManager.Users
            .FirstOrDefaultAsync(u => u.Email == dto.Email);

        if (user == null)
        {
            ModelState.AddModelError("userNotFound", "User is not avaible");
            return NotFound(ModelState);
        }

        var changePasswordResult = await _userManager.ResetPasswordAsync(user, dto.Token, dto.NewPassword);

        if (!changePasswordResult.Succeeded)
        {
            AddErrorsInModelState(changePasswordResult);
            return ValidationProblem();
        }

        var roleName = (await _userManager.GetRolesAsync(user)).FirstOrDefault();

        if (roleName == null) return NotFound("User doesn't have roles");

        var userDto = _mapper.Map<UserFullInfoDto>(user);
        userDto.Role = roleName;

        var token = GetToken(userDto);
        var result = new UserTokenDto
        {
            User = userDto,
            AccessToken = token.Access,
            RefreshToken = token.Refresh
        };

        return Json(result);
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = ApplicationRole.UserAndAdmin)]
    [HttpPost("password/change")]
    public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
    {
        var user = await _userManager.GetUserAsync(User);

        if (user == null) return NotFound();

        var changePasswordResult = await _userManager.ChangePasswordAsync(user, dto.OldPassword, dto.NewPassword);

        if (!changePasswordResult.Succeeded)
        {
            AddErrorsInModelState(changePasswordResult);
            return ValidationProblem();
        }

        return Ok();
    }

    private string GetAccessToken(int id, string role)
    {
        var claims = new Claim[]
        {
            new(ClaimTypes.NameIdentifier, id.ToString()),
            new(ClaimTypes.Role, role)
        };

        var access = _tokenGenerator.GenerateToken(claims, TokenType.Access);

        return access;
    }

    private string GetRefreshToken(int id)
    {
        var claims = new Claim[]
        {
            new(ClaimTypes.NameIdentifier, id.ToString()),
            new(ClaimTypes.Role, "refresh")
        };

        var access = _tokenGenerator.GenerateToken(claims, TokenType.Refresh);

        return access;
    }

    private (string Access, string Refresh) GetToken(UserFullInfoDto user)
    {
        return GetToken(user.Id, user.Role);
    }

    private (string Access, string Refresh) GetToken(ApplicationUser user, string role)
    {
        return GetToken(user.Id, role);
    }

    private (string Access, string Refresh) GetToken(int id, string role)
    {
        var access = GetAccessToken(id, role);
        var refresh = GetRefreshToken(id);

        return (access, refresh);
    }


    private void AddErrorsInModelState(IdentityResult identityResult)
    {
        AddErrorsInModelState(identityResult.Errors);
    }

    private void AddErrorsInModelState(IEnumerable<IdentityError> errors)
    {
        foreach (var error in errors) ModelState.AddModelError(error.Code, error.Description);
    }
}