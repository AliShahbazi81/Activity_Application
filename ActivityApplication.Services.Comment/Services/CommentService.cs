using ActivityApplication.DataAccess.DbContext;
using ActivityApplication.Domain.Results;
using ActivityApplication.Services.Comment.Dto;
using Microsoft.EntityFrameworkCore;

namespace ActivityApplication.Services.Comment.Services;

public class CommentService : ICommentService
{
    private readonly IDbContextFactory<ApplicationDbContext> _context;

    public CommentService(IDbContextFactory<ApplicationDbContext> context)
    {
        _context = context;
    }

    public async Task<Result<CommentDto>> CreateCommentAsync(
        Guid activityId,
        Guid userId,
        string body)
    {
        await using var dbContext = await _context.CreateDbContextAsync();

        var activity = await dbContext.Activities
            .SingleOrDefaultAsync(a => a.Id == activityId);

        if (activity == null)
            return Result<CommentDto>.Failure("Failed to find the activity");

        var user = await dbContext.Users
            .Include(p => p.Photos)
            .Where(u => u.Id == userId)
            .SingleOrDefaultAsync();

        var mainPhoto = user.Photos.Any()
            ? user?.Photos?.SingleOrDefault(p => p.IsMain)
            : null;

        var comment = new DataAccess.Entities.Comments.Comment
        {
            Author = user,
            Activity = activity,
            Body = body
        };

        dbContext.Comments.Add(comment);

        var saved = await dbContext.SaveChangesAsync() > 0;

        if (!saved)
            return Result<CommentDto>.Failure("Failed to save the data");

        var createdDto = new CommentDto
        {
            CreatedAt = comment.CreatedAt,
            Body = comment.Body,
            Username = user.UserName,
            DisplayName = user.DisplayName,
            Image = mainPhoto.Url
        };

        return Result<CommentDto>.Success(createdDto);
    }

    public async Task<Result<IEnumerable<CommentDto>>> GetComments(Guid activityId)
    {
        await using var dbContext = await _context.CreateDbContextAsync();

        var comments = await dbContext.Comments
            .Include(c => c.Author)
            .ThenInclude(user => user.Photos)
            .Where(x => x.Activity.Id == activityId)
            .OrderBy(x => x.CreatedAt)
            .ToListAsync();


        var createDto = comments.Select(x => new CommentDto
        {
            Id = x.Id,
            Body = x.Body,
            CreatedAt = x.CreatedAt,
            Username = x.Author.UserName,
            DisplayName = x.Author.DisplayName,
            Image = x.Author.Photos.Where(x => x.IsMain == true).Select(x => x.Url).SingleOrDefault()
        }).ToList();

        return Result<IEnumerable<CommentDto>>.Success(createDto);
    }
}