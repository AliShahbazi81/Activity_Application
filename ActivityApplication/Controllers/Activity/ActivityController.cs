using System.Net;
using System.Runtime.InteropServices;
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

    [HttpGet("GetActivity")]
    public async Task<ActivityDto?> GetActivity(Guid activityId)
    {
        try
        {
            return await _service.GetActivityAsync(activityId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpPost("Create")]
    public async Task<ActivityDto?> CreateActivity(ActivityDto activityDto)
    {
        try
        {
            if (!ModelState.IsValid)
                throw new Exception("Error in data's format!");

            var createActivity = await _service.CreateActivityAsync(activityDto);
            return createActivity;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpPut("Update")]
    public async Task<ActionResult<ActivityDto?>> UpdateActivity(
        Guid activityId,
        ActivityDto activityDto)
    {
        try
        {
            if (!ModelState.IsValid)
                throw new Exception("Error in data's format!");

            var update = await _service.UpdateActivityAsync(activityId, activityDto);

            if (update)
                return await _service.GetActivityAsync(activityId);

            return NotFound();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpDelete("Delete")]
    public async Task<ActionResult> DeleteActivity(Guid activityId)
    {
        try
        {
            var delete = await _service.DeleteActivityAsync(activityId);

            if (delete)
                return Ok();
            return BadRequest("Error deleting the activity!");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}