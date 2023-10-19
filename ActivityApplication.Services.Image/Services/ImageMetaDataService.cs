using ActivityApplication.DataAccess.DbContext;
using ActivityApplication.DataAccess.Entities.Users;
using ActivityApplication.Domain.Results;
using ActivityApplication.Infrastructure.Security;
using ActivityApplication.Services.DTO;
using Microsoft.EntityFrameworkCore;

namespace ActivityApplication.Services.Image.Services;

public class ImageMetaDataService : IImageMetaDataService
{
    private readonly IDbContextFactory<ApplicationDbContext> _context;
    private readonly IUserAccessor _userAccessor;

    public ImageMetaDataService(
        IDbContextFactory<ApplicationDbContext> context,
        IUserAccessor userAccessor)
    {
        _context = context;
        _userAccessor = userAccessor;
    }

    public async Task<bool> UploadAsync(ImageUploadDto? uploadDto)
    {
        await using var dbContext = await _context.CreateDbContextAsync();

        if (uploadDto is { Url: not null, PublicId: not null })
            _ = dbContext.Add(new Photo
            {
                PublicId = uploadDto.PublicId,
                Url = uploadDto.Url,
                IsMain = false,

                UserId = _userAccessor.GetUserId()
            });

        var isSaved = await dbContext.SaveChangesAsync() > 0;

        return isSaved;
    }
}