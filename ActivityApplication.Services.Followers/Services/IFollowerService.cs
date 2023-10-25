using ActivityApplication.Domain.Results;

namespace ActivityApplication.Services.Followers.Services;

public interface IFollowerService
{
    Task<Result<string>> FollowAsync(Guid userId, Guid targetUserId);
    Task<Result<string>> UnfollowAsync(Guid userId, Guid targetUserId);
}