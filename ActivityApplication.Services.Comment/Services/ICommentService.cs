using ActivityApplication.Domain.Results;
using ActivityApplication.Services.Comment.Dto;

namespace ActivityApplication.Services.Comment.Services;

public interface ICommentService
{
    Task<Result<CommentDto>> CreateCommentAsync(
        Guid activityId,
        Guid userId,
        CommentDto commentDto);

    Task<Result<IEnumerable<CommentDto>>> GetComments(Guid activityId);
}