using ActivityApplication.Controllers.ActivityControllers;
using ActivityApplication.Infrastrucutre.UploadImage.Services;
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

            return Ok(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}