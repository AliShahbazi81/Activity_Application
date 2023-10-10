using System.Net.Mime;
using System.Security.Claims;
using ActivityApplication.DataAccess.Users;
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
        var user = await _userManager.FindByEmailAsync(loginDto.Email);

        if (user == null) return Unauthorized();

        var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);

        if (result)
            return new UserDto
            {
                Username = user.UserName,
                Image = null,
                Token = await _tokenService.GenerateToken(user.Id),
                DisplayName = user.DisplayName
            };

        return Unauthorized();
    }

    [AllowAnonymous]
    [HttpPost("Register")]
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        if (await _userManager.Users.AnyAsync(x => x.UserName == registerDto.Username))
            return BadRequest("Username is already taken.");

        if (await _userManager.Users.AnyAsync(x => x.Email == registerDto.Email))
            return BadRequest("Email is already taken.");

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
            Image = null,
            Token = await _tokenService.GenerateToken(user.Id),
            Username = user.UserName
        });
    }
}