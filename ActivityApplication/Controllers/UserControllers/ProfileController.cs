using ActivityApplication.Controllers.ActivityControllers;
using ActivityApplication.Infrastructure.Security;
using ActivityApplication.Services.User.Dto;
using ActivityApplication.Services.User.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ActivityApplication.Controllers.UserControllers;

[Authorize]
public class ProfileController : BaseApiController
{
    private readonly IUserService _userService;
    private readonly IUserAccessor _userAccessor;

    public ProfileController(
        IUserService userService,
        IUserAccessor userAccessor)
    {
        _userService = userService;
        _userAccessor = userAccessor;
    }

    [HttpGet("{userName}")]
    public async Task<IActionResult> ProfileDetail(string userName)
    {
        try
        {
            return HandleResult(await _userService.GetUserProfileByUsernameAsync(userName.ToLower()));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpPut("Update/{username}")]
    public async Task<IActionResult> UpdateProfile(UserEditDto userEditDto)
    {
        try
        {
            return HandleResult(await _userService.EditUserProfileById(_userAccessor.GetUserId(), userEditDto));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}