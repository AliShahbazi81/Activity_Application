using ActivityApplication.Services.Activity;
using ActivityApplication.Services.Activity.DTO;
using Microsoft.AspNetCore.Mvc;

namespace ActivityApplication.Controllers.Activity;

public class ActivityController : BaseApiController
{
    private readonly IActivityService _service;

    public ActivityController(IActivityService service)
    {
        _service = service;
    }

    [HttpGet("GetActivities")]
    public async Task<IEnumerable<ActivityDto>> GetActivities()
    {
        try
        {
            var getActivities = await _service.GetActivitiesAsync();

            return getActivities;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}