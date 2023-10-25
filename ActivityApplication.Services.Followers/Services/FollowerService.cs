using ActivityApplication.DataAccess.DbContext;
using ActivityApplication.DataAccess.Entities.Followers;
using ActivityApplication.Domain.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ActivityApplication.Services.Followers.Services;

public class FollowerService : IFollowerService
{
    private readonly IDbContextFactory<ApplicationDbContext> _context;

    public FollowerService(IDbContextFactory<ApplicationDbContext> context)
    {
        _context = context;
    }

    public async Task<Result<string>> FollowAsync(Guid userId, Guid targetUserId)
    {
        await using var dbContext = await _context.CreateDbContextAsync();

        // Check if user exists
        var getUser = await dbContext.Users.FindAsync(userId);

        if (getUser == null)
            return Result<string>.Failure("Failed to authorize user!");

        // Check if both id's are the same
        if (IsUserIdSameAsync(userId, targetUserId))
            return Result<string>.Failure("You cannot follow yourself!");

        var getTargetUser = await dbContext.Users.FindAsync(targetUserId);

        if (getTargetUser == null)
            return Result<string>.Failure("Failed to authorize the user you are trying to follow!");

        // Check if the user us already followed the target
        if (await IsTargetFoundAsync(userId, targetUserId))
            return Result<string>.Failure("You have already followed the user!");

        var follow = new UserFollowing
        {
            ObserverId = userId,
            TargetId = targetUserId
        };

        dbContext.UserFollowings.Add(follow);

        var saved = await dbContext.SaveChangesAsync() > 0;

        return saved
            ? Result<string>.Success("Success!")
            : Result<string>.Failure("Could not follow the user!");
    }

    public async Task<Result<string>> UnfollowAsync(Guid userId, Guid targetUserId)
    {
        await using var dbContext = await _context.CreateDbContextAsync();
        
        // Check if user exists
        var getUser = await dbContext.Users.FindAsync(userId);

        if (getUser == null)
            return Result<string>.Failure("Failed to authorize user!");

        // Check if both id's are the same
        if (IsUserIdSameAsync(userId, targetUserId))
            return Result<string>.Failure("You cannot Unfollow yourself!");

        var getTargetUser = await dbContext.Users.FindAsync(targetUserId);

        if (getTargetUser == null)
            return Result<string>.Failure("Failed to authorize the user you are trying to unfollow!");

        // Check if the user us already followed the target
        if (!await IsTargetFoundAsync(userId, targetUserId))
            return Result<string>.Failure("You cannot unfollow a user who you did not follow!");
        
        var findTargetUser = await dbContext.UserFollowings
            .Where(x => x.ObserverId == userId && x.TargetId == targetUserId)
            .SingleOrDefaultAsync();

        if (findTargetUser == null)
            return Result<string>.Failure("Failed to get the user you are trying to unfollow!");

        dbContext.UserFollowings.Remove(findTargetUser);

        var saved = await dbContext.SaveChangesAsync() > 0;
        
        return saved
            ? Result<string>.Success("Success!")
            : Result<string>.Failure("Could not follow the user!");
    }

    private async Task<bool> IsTargetFoundAsync(Guid userId, Guid targetUserId)
    {
        await using var dbContext = await _context.CreateDbContextAsync();

        var findTargetUser = await dbContext.UserFollowings
            .Where(x => x.ObserverId == userId && x.TargetId == targetUserId)
            .SingleOrDefaultAsync();

        return findTargetUser != null;
    }

    private static bool IsUserIdSameAsync(Guid userId, Guid targetUserId)
    {
        return userId == targetUserId;
    }
}