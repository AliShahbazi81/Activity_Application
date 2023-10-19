using ActivityApplication.DataAccess.DbContext;
using ActivityApplication.DataAccess.Entities.Users;
using ActivityApplication.Domain.Results;
using ActivityApplication.Infrastructure.Security;
using ActivityApplication.Services.DTO;
using CloudinaryDotNet.Actions;
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

    public async Task<Result<ImageUploadDto>> UploadAsync(ImageUploadDto? uploadDto)
    {
        await using var dbContext = await _context.CreateDbContextAsync();

        var user = await dbContext.Users
            .Include(x => x.Photos)
            .SingleAsync(x => x.Id == _userAccessor.GetUserId());

        var newPhoto = new Photo
        {
            PublicId = uploadDto.PublicId,
            Url = uploadDto.Url,

            UserId = _userAccessor.GetUserId()
        };

        // Check if user does not have any main photo
        if (!user.Photos.Any(x => x.IsMain))
            newPhoto.IsMain = true;

        user.Photos.Add(newPhoto);

        var isSaved = await dbContext.SaveChangesAsync() > 0;

        var photoDto = new ImageUploadDto
        {
            IsMain = newPhoto.IsMain,
            PublicId = newPhoto.PublicId,
            Url = newPhoto.Url
        };

        return isSaved ? Result<ImageUploadDto>.Success(photoDto) : Result<ImageUploadDto>.Failure("Problem while uploading the photo!");
    }

    public async Task<Result<string>> DeleteAsync(string publicId)
    {
        await using var dbContext = await _context.CreateDbContextAsync();

        var getPhoto = await dbContext.Users
            .Where(x => x.Id == _userAccessor.GetUserId())
            .SelectMany(x => x.Photos)
            .SingleAsync(x => x.PublicId == publicId);


        dbContext.Remove(getPhoto);

        var saved = await dbContext.SaveChangesAsync() > 0;

        return saved ? Result<string>.Success("Photo has been deleted successfully!") : Result<string>.Failure("Error while deleting the photo!");
    }
}