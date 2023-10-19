using ActivityApplication.Domain.Results;
using ActivityApplication.Services.DTO;
using Microsoft.AspNetCore.Http;

namespace ActivityApplication.Infrastrucutre.UploadImage.Services;

public interface IImageManager
{
    Task<Result<ImageUploadDto>> UploadAsync(IFormFile file);

    Task<Result<string>> DeleteAsync(string publicId);
}