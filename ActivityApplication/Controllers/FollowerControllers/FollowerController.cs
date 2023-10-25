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

    [HttpPost("{id}")]
    public async Task<IActionResult> Follow(Guid id)
    {
        try
        {
            return HandleResult(await _followerService.FollowAsync(_userAccessor.GetUserId(), id));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}