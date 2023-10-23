using ActivityApplication.Infrastructure.Security;
using ActivityApplication.Services.Comment.Dto;
using ActivityApplication.Services.Comment.Services;
using Microsoft.AspNetCore.SignalR;

namespace ActivityApplication.SignalR;

public class CommentHub : Hub
{
    private readonly ICommentService _commentService;
    private readonly ILogger<CommentHub> _logger;
    private readonly IUserAccessor _userAccessor;

    public CommentHub(
        ICommentService commentService,
        ILogger<CommentHub> logger, IUserAccessor userAccessor)
    {
        _commentService = commentService;
        _logger = logger;
        _userAccessor = userAccessor;
    }

    public async Task SendComment(string body, string activityIdStr)
    {
        try
        {
            // Convert the activity ID string to Guid
            if (!Guid.TryParse(activityIdStr, out var activityId))
            {
                _logger.LogError("Invalid activityId: {activityIdStr}", activityIdStr);
                await Clients.Caller.SendAsync("ReceiveCommentError", "Invalid activity ID");
                return;
            }

            // Create the comment using your service
            var commentResult = await _commentService.CreateCommentAsync(activityId, _userAccessor.GetUserId(), body);

            if (commentResult.IsSuccess)
            {
                // Send the comment back to the clients in the group
                await Clients.Group(activityIdStr)
                    .SendAsync("ReceiveComment", commentResult.Value); // Assuming the DTO matches what the client expects
            }
            else
            {
                _logger.LogError("Failed to create comment: {Error}", commentResult.Error);
                await Clients.Caller.SendAsync("ReceiveCommentError", "Failed to create comment");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while sending the comment.");
            await Clients.Caller.SendAsync("ReceiveCommentError", "An unexpected error occurred");
        }
    }

    public override async Task OnConnectedAsync()
    {
        var httpContext = Context.GetHttpContext();
        var activityIdStr = httpContext.Request.Query["activityId"].ToString();

        if (Guid.TryParse(activityIdStr, out var activityId))
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, activityIdStr);

            var result = await _commentService.GetComments(activityId);

            if (result.IsSuccess)
            {
                await Clients.Caller.SendAsync("LoadComments", result.Value);
            }
            else
            {
                _logger.LogError("Failed to load comments for activityId {activityId}", activityId);
                await Clients.Caller.SendAsync("LoadCommentsError", "Failed to load comments");
            }
        }
        else
        {
            _logger.LogError("Invalid activityId: {activityIdStr}", activityIdStr);
            await Clients.Caller.SendAsync("LoadCommentsError", "Invalid activity ID");
        }
    }
}