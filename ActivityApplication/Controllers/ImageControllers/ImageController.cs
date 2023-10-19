using ActivityApplication.Controllers.ActivityControllers;
using ActivityApplication.Infrastrucutre.UploadImage.Services;
using ActivityApplication.Services.Image.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ActivityApplication.Controllers.ImageControllers;

[Authorize]
public class ImageController : BaseApiController
{
    private readonly IImageManager _imageManager;

    public ImageController(IImageManager imageManager)
    {
        _imageManager = imageManager;
    }

    [HttpPost]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        try
        {
            var result = await _imageManager.UploadAsync(file);

            return HandleResult(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpDelete("{publicId}")]
    public async Task<IActionResult> Delete(string publicId)
    {
        try
        {
            return HandleResult(await _imageManager.DeleteAsync(publicId));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpPut("{publicId}/setMain")]
    public async Task<IActionResult> SetMain(
        string publicId,
        [FromServices] IImageMetaDataService imageMetaData = null)
    {
        try
        {
            return HandleResult(await imageMetaData.SetMainPhotoAsync(publicId));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}