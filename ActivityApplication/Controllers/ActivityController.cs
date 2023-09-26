using ActivityApplication.Services.Activity;
using ActivityApplication.Services.Activity.DTO;
using ActivityApplication.Services.Activity.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace ActivityApplication.Controllers;

public class ActivityController : BaseApiController
{
    private readonly ILogger<ActivityController> _logger;
    private readonly IActivityService _service;

    public ActivityController(ILogger<ActivityController> logger, IActivityService service)
    {
        _logger = logger;
        _service = service;
    }

    [HttpGet("Get/{activityId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ActivityDto>> GetActivity(Guid activityId)
    {
        try
        {
            var getActivity = await _service.GetActivityAsync(activityId);
            // ReSharper disable once HeapView.BoxingAllocation
            return Ok(getActivity);
        }
        catch (IdNotFoundException e)
        {
            _logger.LogError("Error getting the activity! ", e.Message);
            throw;
        }
        catch (Exception e)
        {
            _logger.LogError("Error getting the activity: ", e.Message);
            throw;
        }
    }

    [HttpGet("Get")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IEnumerable<ActivityDto>?> GetActivities()
    {
        try
        {
            var getActivities = await _service.GetActivitiesAsync();

            return getActivities;
        }
        catch (Exception e)
        {
            _logger.LogError("Error on getting the activities: ", e.Message);
            throw;
        }
    }

    [HttpPost("Create")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateActivity(ActivityDto activityDto)
    {
        try
        {
            var activity = await _service.CreateActivityAsync(activityDto);
            return Ok(activity);
        }
        catch (DateTimeValidationException e)
        {
            _logger.LogError("Date input validation error! ", e.Message);
            throw;
        }
        catch (StringLengthException e)
        {
            _logger.LogError("Error on string length!", e.Message);
            throw;
        }
        catch (Exception e)
        {
            _logger.LogError("Error on creating the activity: ", e.Message);
            throw;
        }
    }

    [HttpPut("Update")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> UpdateActivity(Guid activityId, ActivityDto activityDto)
    {
        try
        {
            _ = await _service.UpdateActivityAsync(activityId, activityDto);

            return Ok("The activity has been saved successfully!");
        }
        catch (IdNotFoundException e)
        {
            _logger.LogError("Id not found! ", e.Message);
            throw;
        }
        catch (DateTimeValidationException e)
        {
            _logger.LogError("Date input validation error! ", e.Message);
            throw;
        }
        catch (StringLengthException e)
        {
            _logger.LogError("Error on string length!", e.Message);
            throw;
        }
        catch (Exception e)
        {
            _logger.LogError("Error on creating the activity: ", e.Message);
            throw;
        }
    }

    [HttpDelete("Delete")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteActivity(Guid activityId)
    {
        try
        {
            _ = await _service.DeleteActivityAsync(activityId);

            return Ok("The activity has been deleted successfully!");
        }
        catch (IdNotFoundException e)
        {
            _logger.LogError("Id cannot be found! ", e.Message);
            throw;
        }
        catch (Exception e)
        {
            _logger.LogError("Error on deleting the activity!", e.Message);
            throw;
        }
    }
}