using ActivityApplication.DataAccess.DbContext;
using ActivityApplication.Domain.Results;
using ActivityApplication.Services.Image.DTO;
using Microsoft.EntityFrameworkCore;

namespace ActivityApplication.Services.Image.Services;

public class ImageService : IImageService
{
    private readonly IDbContextFactory<ApplicationDbContext> _context;

    public ImageService(IDbContextFactory<ApplicationDbContext> context)
    {
        _context = context;
    }

    public async Task<Result<string>> UploadPhotoAsync(ImageUploadDto file)
    {
        await using var dbContext = await _context.CreateDbContextAsync();
        
    }
}