using ActivityApplication.DataAccess.DbContext;
using ActivityApplication.Domain.Results;
using ActivityApplication.Services.DTO;
using Microsoft.EntityFrameworkCore;

namespace ActivityApplication.Services.User.Services;

public class UserService : IUserService
{
    private readonly IDbContextFactory<ApplicationDbContext> _context;

    public UserService(IDbContextFactory<ApplicationDbContext> context)
    {
        _context = context;
    }

    public async Task<Result<ProfileDto>> GetUserProfileByUsernameAsync(string userName)
    {
        await using var dbContext = await _context.CreateDbContextAsync();

        var getUser = await dbContext.Users
            .Where(x => x.UserName == userName)
            .Include(x => x.Photos)
            .SingleOrDefaultAsync();

        if (getUser == null)
            return Result<ProfileDto>.Failure("Failed to find the user!");

        var profileDto = await MapToProfileDto(getUser, userName);

        return Result<ProfileDto>.Success(profileDto);
    }

    private async Task<ProfileDto?> MapToProfileDto(DataAccess.Entities.Users.User userEntity, string userName)
    {
        return new ProfileDto
        {
            Username = userEntity.UserName,
            DisplayName = userEntity.DisplayName,
            Bio = userEntity.Bio,
            Image = await GetUserMainPhotoUrlAsync(userName),
            Photos = userEntity.Photos.Any()
                ? userEntity.Photos.Select(p => new ImageUploadDto
                {
                    PublicId = p.PublicId,
                    Url = p.Url,
                    IsMain = p.IsMain
                }).ToList()
                : null
        };
    }

    private async Task<string?> GetUserMainPhotoUrlAsync(string userName)
    {
        await using var dbContext = await _context.CreateDbContextAsync();

        // Since user's photo can be null, we use SingleOrDefaultAsync.
        var photoUrl = await dbContext.Users
            .Where(x => x.UserName == userName)
            .SelectMany(x => x.Photos)
            .Where(x => x.IsMain)
            .Select(x => x.Url)
            .SingleOrDefaultAsync();

        return string.IsNullOrEmpty(photoUrl) ? null : photoUrl;
    }
}