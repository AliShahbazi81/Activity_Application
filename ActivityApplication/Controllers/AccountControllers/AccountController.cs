using ActivityApplication.DataAccess.Users;
using ActivityApplication.Services.User.DTOs;
using ActivityApplication.Services.User.Services.Token;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ActivityApplication.Controllers.AccountControllers;

[AllowAnonymous]
[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly TokenService _tokenService;

    public AccountController(UserManager<User> userManager, TokenService tokenService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
    }

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
}