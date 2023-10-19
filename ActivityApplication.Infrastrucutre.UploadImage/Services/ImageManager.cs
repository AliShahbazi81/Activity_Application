using ActivityApplication.Domain.Results;
using ActivityApplication.Infrastructure.Courdinary.Services.Accessor;
using ActivityApplication.Services.Image.Services;
using Microsoft.AspNetCore.Http;

namespace ActivityApplication.Infrastrucutre.UploadImage.Services;

public class ImageManager : IImageManager
{
    private readonly IImageMetaDataService _imageMetaDataService;
    private readonly ICloudinaryImageService _cloudinaryImageService;

    public ImageManager(IImageMetaDataService imageMetaDataService, ICloudinaryImageService cloudinaryImageService)
    {
        _imageMetaDataService = imageMetaDataService;
        _cloudinaryImageService = cloudinaryImageService;
    }

    public async Task<Result<string>> UploadAsync(IFormFile file)
    {
        var cloudinaryUpload = await _cloudinaryImageService.AddPhotoAsync(file);

        if (cloudinaryUpload != null)
            await _imageMetaDataService.UploadAsync(cloudinaryUpload);

        return Result<string>.Success("Image has been uploaded successfully");
    }
}