using ActivityApplication.DataAccess.DbContext;
using Microsoft.EntityFrameworkCore;

namespace ActivityApplication.Services.Services;

public class CommonService : ICommonService
{
    private readonly IDbContextFactory<ApplicationDbContext> _context;

    public CommonService(IDbContextFactory<ApplicationDbContext> context)
    {
        _context = context;
    }

    public async Task<string?> GetUserMainPhotoAsync(Guid userId)
    {
        await using var dbContext = await _context.CreateDbContextAsync();

        var mainPhotoUrl = await dbContext.Users
            .Where(u => u.Id == userId)
            .SelectMany(u => u.Photos.Where(p => p.IsMain))
            .Select(p => p.Url)
            .SingleOrDefaultAsync();

        return mainPhotoUrl;
    }
}