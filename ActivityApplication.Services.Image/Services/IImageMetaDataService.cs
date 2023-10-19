using ActivityApplication.Services.DTO;

namespace ActivityApplication.Services.Image.Services;

public interface IImageMetaDataService
{
    /// <summary>
    /// Upload data returned from Cloudinary to Database
    /// </summary>
    /// <param name="uploadDto"></param>
    /// <returns>bool</returns>
    Task<bool> UploadAsync(ImageUploadDto? uploadDto);
}