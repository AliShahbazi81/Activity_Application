using System.Net.Mime;
using System.Security.Claims;
using ActivityApplication.DataAccess.Entities.Users;
using ActivityApplication.Services.User.DTOs;
using ActivityApplication.Services.User.Services.Token;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ActivityApplication.Controllers.AccountControllers;

[ApiController]
[Route("api/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class AccountController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly TokenService _tokenService;

    public AccountController(UserManager<User> userManager, TokenService tokenService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
    }

    [AllowAnonymous]
    [HttpPost("Login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        var user = await _userManager.Users
            .Include(x => x.Photos)
            .FirstOrDefaultAsync(x => x.Email == loginDto.Email);

        if (user == null) return Unauthorized();

        var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);

        if (!result) return Unauthorized();

        var mainPhotoUrl = user.Photos.Any()
            ? user.Photos
                .Where(photo => photo.UserId == user.Id && photo.IsMain)
                .Select(p => p.Url)
                .SingleOrDefault()
            : null;

        return new UserDto
        {
            Username = user.UserName,
            Image = mainPhotoUrl,
            Token = await _tokenService.GenerateToken(user.Id),
            DisplayName = user.DisplayName
        };
    }

    [AllowAnonymous]
    [HttpPost("Register")]
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        if (await _userManager.Users.AnyAsync(x => x.UserName == registerDto.Username))
        {
            ModelState.AddModelError("username", "Username is taken");
            return ValidationProblem();
        }

        // Using ValidationProblem so that we can show the proper error in our client-side components
        if (await _userManager.Users.AnyAsync(x => x.Email == registerDto.Email))
        {
            ModelState.AddModelError("email", "Email is already taken");
            return ValidationProblem();
        }


        var user = new User
        {
            DisplayName = registerDto.DisplayName,
            Email = registerDto.Email,
            UserName = registerDto.Username
        };

        var result = await _userManager.CreateAsync(user, registerDto.Password);

        if (result.Succeeded) return await CreateUserObject(user);

        return BadRequest(result.Errors);
    }

    [Authorize]
    [HttpGet("GetCurrentUser")]
    public async Task<ActionResult<UserDto>> GetCurrentUser()
    {
        var user = await _userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));

        return await CreateUserObject(user!);
    }


    private async Task<ActionResult<UserDto>> CreateUserObject(User user)
    {
        return new JsonResult(new UserDto
        {
            DisplayName = user.DisplayName,
            Image = string.Empty,
            Token = await _tokenService.GenerateToken(user.Id),
            Username = user.UserName
        });
    }
}