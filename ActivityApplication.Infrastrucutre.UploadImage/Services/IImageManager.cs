using ActivityApplication.Domain.Results;
using Microsoft.AspNetCore.Http;

namespace ActivityApplication.Infrastrucutre.UploadImage.Services;

public interface IImageManager
{
    Task<Result<string>> UploadAsync(IFormFile file);
}