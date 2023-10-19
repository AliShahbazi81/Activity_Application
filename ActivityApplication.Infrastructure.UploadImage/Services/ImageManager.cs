using ActivityApplication.DataAccess.Entities.Users;
using ActivityApplication.Domain.Results;
using ActivityApplication.Infrastructure.Courdinary.Services.Accessor;
using ActivityApplication.Services.DTO;
using ActivityApplication.Services.Image.Services;
using CloudinaryDotNet.Actions;
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

    public async Task<Result<ImageUploadDto>> UploadAsync(IFormFile file)
    {
        var cloudinaryUpload = await _cloudinaryImageService.AddPhotoAsync(file);

        return await _imageMetaDataService.UploadAsync(cloudinaryUpload);
    }

    public async Task<Result<string>> DeleteAsync(string publicId)
    {
        var cloudinaryDeletion = await _cloudinaryImageService.DeletePhotoAsync(publicId);

        if (cloudinaryDeletion is null)
            return Result<string>.Failure("Error while deleting the photo!");

        return await _imageMetaDataService.DeleteAsync(publicId);
    }
}