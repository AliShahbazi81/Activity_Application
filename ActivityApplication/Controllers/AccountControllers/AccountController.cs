using ActivityApplication.DataAccess.Users;
using ActivityApplication.Services.User.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ActivityApplication.Controllers.AccountControllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly UserManager<User> _userManager;

    public AccountController(UserManager<User> userManager)
    {
        _userManager = userManager;
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
                Token = "This will be replaced by JWT",
                DisplayName = user.DisplayName
            };

        return Unauthorized();
    }
}