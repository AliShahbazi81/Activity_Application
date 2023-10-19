using ActivityApplication.Infrastructure.Courdinary.Settings;
using ActivityApplication.Services.DTO;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace ActivityApplication.Infrastructure.Courdinary.Services.Accessor;

public class CloudinaryImageService : ICloudinaryImageService
{
    private readonly Cloudinary _cloudinary;

    public CloudinaryImageService(IOptions<CloudinarySettings> config)
    {
        // Since we are using parentheses, the ordering is important. This is not object 
        var account = new Account(
            config.Value.CloudName,
            config.Value.ApiKey,
            config.Value.SecretKey
        );
        _cloudinary = new Cloudinary(account);
    }

    public async Task<ImageUploadDto?> AddPhotoAsync(IFormFile file)
    {
        if (file.Length <= 0) return null;

        await using var stream = file.OpenReadStream();
        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(file.FileName, stream),
            Transformation = new Transformation().Height(500).Width(500).Crop("fill")
        };

        var uploadResult = await _cloudinary.UploadAsync(uploadParams);

        if (uploadResult.Error != null)
            throw new Exception(uploadResult.Error.Message);

        return new ImageUploadDto
        {
            PublicId = uploadResult.PublicId,
            // If the application wants to be placed in HTTPS, we use SecureURL
            Url = uploadResult.SecureUrl.ToString()
        };
    }

    public async Task<string?> DeletePhotoAsync(string publicId)
    {
        var deleteParams = new DeletionParams(publicId);
        var result = await _cloudinary.DestroyAsync(deleteParams);
        return result.Result == "Ok" ? result.Result : null;
    }
}