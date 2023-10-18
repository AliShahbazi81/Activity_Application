using ActivityApplication.Services.Image.DTO;
using Microsoft.AspNetCore.Http;

namespace ActivityApplication.Services.Image.Services.Accessor;

public interface IImageAccessor
{
    Task<ImageUploadDto?> AddPhotoAsync(IFormFile file);
    Task<string?> DeletePhotoAsync(string publicId);
}