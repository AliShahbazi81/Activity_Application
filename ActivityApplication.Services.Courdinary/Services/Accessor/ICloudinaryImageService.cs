using ActivityApplication.Services.DTO;
using Microsoft.AspNetCore.Http;

namespace ActivityApplication.Infrastructure.Courdinary.Services.Accessor;

public interface ICloudinaryImageService
{
    Task<ImageUploadDto?> AddPhotoAsync(IFormFile file);
    Task<string?> DeletePhotoAsync(string publicId);
}