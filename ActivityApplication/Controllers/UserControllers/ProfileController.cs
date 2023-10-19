using ActivityApplication.Controllers.ActivityControllers;
using ActivityApplication.Services.User.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ActivityApplication.Controllers.UserControllers;

[Authorize]
public class ProfileController : BaseApiController
{
    private readonly IUserService _userService;

    public ProfileController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("/profiles/{userName}")]
    public async Task<IActionResult> ProfileDetail(string userName)
    {
        try
        {
            return HandleResult(await _userService.GetUserProfileByUsernameAsync(userName));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}