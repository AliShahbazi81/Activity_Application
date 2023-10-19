using ActivityApplication.DataAccess.Entities.Users;
using ActivityApplication.Domain.Results;
using ActivityApplication.Services.DTO;

namespace ActivityApplication.Services.Image.Services;

public interface IImageMetaDataService
{
    /// <summary>
    /// Upload data returned from Cloudinary to Database
    /// </summary>
    /// <param name="uploadDto"></param>
    /// <returns>bool</returns>
    Task<Result<ImageUploadDto>> UploadAsync(ImageUploadDto? uploadDto);

    /// <summary>
    /// Delete photo based on the publicId returned from Cloudinary service
    /// </summary>
    /// <param name="publicId"></param>
    /// <returns>String Result</returns>
    Task<Result<string>> DeleteAsync(string publicId);
}