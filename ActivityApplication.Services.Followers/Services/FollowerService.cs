using ActivityApplication.DataAccess.DbContext;
using ActivityApplication.DataAccess.Entities.Followers;
using ActivityApplication.Domain.Results;
using ActivityApplication.Services.DTO;
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

        // Check if both id's are the same
        if (IsUserIdSameAsync(userId, targetUserId))
            return Result<string>.Failure("You cannot follow yourself!");

        // Check if user exists
        var following = await dbContext.UserFollowings.FindAsync(userId, targetUserId);

        if (following == null)
        {
            var follow = new UserFollowing
            {
                ObserverId = userId,
                TargetId = targetUserId
            };
            dbContext.UserFollowings.Add(follow);
        }
        else
        {
            dbContext.UserFollowings.Remove(following);
        }

        var saved = await dbContext.SaveChangesAsync() > 0;

        return saved
            ? Result<string>.Success("Success!")
            : Result<string>.Failure("Could not follow the user!");
    }

    public async Task<Result<List<ProfileDto>>> GetFollowersListAsync(Guid userId)
    {
        await using var dbContext = await _context.CreateDbContextAsync();

        var userWithDetails = await dbContext.Users
            .Where(u => u.Id == userId)
            .Select(u => new
            {
                User = u,
                MainPhotoUrl = u.Photos.Where(p => p.IsMain).Select(p => p.Url).FirstOrDefault(),
                u.Followers,
                u.Followings
            })
            .ToListAsync();
        
        var createDto = userWithDetails.Select(x => new ProfileDto
        {
            
        })
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