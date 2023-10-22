using ActivityApplication.Controllers.ActivityControllers;
using ActivityApplication.Infrastructure.Security;
using ActivityApplication.Services.Comment.Dto;
using ActivityApplication.Services.Comment.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ActivityApplication.Controllers.CommentControllers;

[Authorize]
public class CommentController : BaseApiController
{
    private readonly ICommentService _commentService;
    private readonly IUserAccessor _userAccessor;

    public CommentController(ICommentService commentService, IUserAccessor userAccessor)
    {
        _commentService = commentService;
        _userAccessor = userAccessor;
    }

    [HttpGet]
    public async Task<IActionResult> GetComments(Guid activityId)
    {
        try
        {
            return HandleResult(await _commentService.GetComments(activityId));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(Guid activityId, CommentDto commentDto)
    {
        try
        {
            return HandleResult(await _commentService.CreateCommentAsync(activityId, _userAccessor.GetUserId(), commentDto));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}