using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Authentications.Facebook;
using OnlineStore.Entities;

namespace OnlineStore.Controllers;

[ApiController]
[Route("api/v1/user")]
public class UserSignController : Controller
{
    private readonly IMapper _mapper;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ITokenGenerator _tokenGenerator;
    private readonly UserManager<ApplicationUser> _userManager;

    public UserSignController(UserManager<ApplicationUser> userManager, IMapper mapper,
        SignInManager<ApplicationUser> signInManager, ITokenGenerator tokenGenerator)
    {
        _userManager = userManager;
        _mapper = mapper;
        _signInManager = signInManager;
        _tokenGenerator = tokenGenerator;
    }

    #region Authorization

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
        var user = await CreateUser(dto);

        if (user == null)
            return ValidationProblem();

        var userTokenDto = GenerateUserTokenDto(user);

        return Json(userTokenDto);
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
        var user = await FindUser(dto);

        if (user == null)
            return ValidationProblem();

        var roleName = (await _userManager.GetRolesAsync(user)).FirstOrDefault();

        if (roleName == null)
            return NotFound("User doesn't have roles");

        var userTokenDto = GenerateUserTokenDto(user, roleName);

        return Json(userTokenDto);
    }

    private async Task<ApplicationUser?> FindUser(LoginDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);

        if (user == null)
        {
            ModelState.AddModelError("userNotFound", "User is not avaible");
            return null;
        }

        var signInResult =
            await _signInManager.PasswordSignInAsync(user, dto.Password, false, false);

        if (!signInResult.Succeeded)
        {
            ModelState.AddModelError("passwordIsNotValid", "Password is not valid");
            return null;
        }

        return user;
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

        var result = GeneratyUserAccessTokenDto(user, role);

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
        var user = await _userManager.FindByEmailAsync(dto.Email);

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

        var result = GenerateUserTokenDto(user, roleName);

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

    #endregion

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
        var email = User.FindFirstValue(ClaimTypes.Email);
        //TODO: change not found email
        if (string.IsNullOrEmpty(email))
            return Unauthorized("email is empty");

        var user = await _userManager.FindByEmailAsync(email);
        string roleName;

        if (user == null)
        {
            var registrationDto = new RegistrationDto
            {
                Email = email,
                FirstName = User.FindFirstValue(ClaimTypes.GivenName),
                LastName = User.FindFirstValue(ClaimTypes.Surname)
            };

            user = await CreateUser(registrationDto);
            if (user == null)
                return ValidationProblem();

            roleName = ApplicationRole.User;
        }
        else
        {
            roleName = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
            if (roleName == null)
                return NotFound("User doesn't have roles");
        }

        var result = GenerateUserTokenDto(user, roleName);
        return Json(result);
    }

    #endregion
    
    #region Another methods

    /// <summary>
    ///     Generates a UserAccessTokenDto containing user information and access token based on the provided ApplicationUser
    ///     and role.
    /// </summary>
    /// <param name="user">The ApplicationUser object representing the user for which the access token is generated.</param>
    /// <param name="role">The role to be assigned to the user.</param>
    /// <returns>A UserAccessTokenDto containing user information and access token.</returns>
    private UserAccessTokenDto GeneratyUserAccessTokenDto(ApplicationUser user, string role)
    {
        var token = GetAccessToken(user.Id, role);
        var userDto = _mapper.Map<UserFullInfoDto>(user);
        userDto.Role = role;

        var result = new UserAccessTokenDto
        {
            AccessToken = token,
            User = userDto
        };
        return result;
    }

    /// <summary>
    ///     Generates a UserTokenDto containing user information, access token, and refresh token based on the provided
    ///     ApplicationUser and role.
    /// </summary>
    /// <param name="user">The ApplicationUser object representing the user for which the tokens are generated.</param>
    /// <param name="role">
    ///     The role to be assigned to the user. Defaults to <see cref="ApplicationRole.User" /> if not
    ///     provided.
    /// </param>
    private UserTokenDto GenerateUserTokenDto(ApplicationUser user, string role = ApplicationRole.User)
    {
        var token = GetAccessAndRefreshToken(user, role);
        var userResult = _mapper.Map<UserFullInfoDto>(user);
        userResult.Role = role;

        var response = new UserTokenDto
        {
            User = userResult,
            AccessToken = token.Access,
            RefreshToken = token.Refresh
        };
        return response;
    }

    /// <summary>
    ///     Creates a new user based on the data from the <paramref name="dto" /> object and assigns the '
    ///     <see cref="ApplicationRole.User" />' role to the user.
    /// </summary>
    /// <param name="dto">Data for registering a new user. The Password field can be empty or null.</param>
    /// <returns>
    ///     An <see cref="ApplicationUser" /> object representing the created user or null if the creation was unsuccessful.
    /// </returns>
    private async Task<ApplicationUser?> CreateUser(RegistrationDto dto)
    {
        var user = _mapper.Map<ApplicationUser>(dto);
        user.UserName = $"user{Guid.NewGuid():N}";

        var userCreateResult = string.IsNullOrEmpty(dto.Password)
            ? await _userManager.CreateAsync(user)
            : await _userManager.CreateAsync(user, dto.Password);

        if (!userCreateResult.Succeeded)
        {
            AddErrorsInModelState(userCreateResult);
            return null;
        }

        var roleAddResult = await _userManager.AddToRoleAsync(user, ApplicationRole.User);

        if (!roleAddResult.Succeeded)
        {
            AddErrorsInModelState(userCreateResult);
            return null;
        }

        return user;
    }

    private string GetToken(int id, string role, TokenType tokenType)
    {
        var claims = new Claim[]
        {
            new(ClaimTypes.NameIdentifier, id.ToString()),
            new(ClaimTypes.Role, role)
        };

        var token = _tokenGenerator.GenerateToken(claims, TokenType.Access);
        return token;
    }

    private string GetAccessToken(int id, string role)
    {
        return GetToken(id, role, TokenType.Access);
    }

    private string GetRefreshToken(int id)
    {
        return GetToken(id, "refresh", TokenType.Refresh);
    }

    private (string Access, string Refresh) GetAccessAndRefreshToken(ApplicationUser user, string role)
    {
        return GetAccessAndRefreshToken(user.Id, role);
    }

    private (string Access, string Refresh) GetAccessAndRefreshToken(int id, string role)
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

    #endregion
}