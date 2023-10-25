using ActivityApplication.Controllers.ActivityControllers;
using ActivityApplication.Infrastructure.Security;
using ActivityApplication.Services.Followers.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ActivityApplication.Controllers.FollowerControllers;

[Authorize]
public class FollowerController : BaseApiController
{
    private readonly IUserAccessor _userAccessor;
    private readonly IFollowerService _followerService;

    public FollowerController(IUserAccessor userAccessor, IFollowerService followerService)
    {
        _userAccessor = userAccessor;
        _followerService = followerService;
    }

    [HttpPost("{targetUserId}")]
    public async Task<IActionResult> Follow(Guid targetUserId)
    {
        try
        {
            return HandleResult(await _followerService.FollowAsync(_userAccessor.GetUserId(), targetUserId));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpDelete("{targetUserId}")]
    public async Task<IActionResult> Unfollow(Guid targetUserId)
    {
        try
        {
            return HandleResult(await _followerService.UnfollowAsync(_userAccessor.GetUserId(), targetUserId));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}