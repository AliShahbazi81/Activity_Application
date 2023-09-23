using ActivityApplication.Services.Activity;
using ActivityApplication.Services.Activity.DTO;
using ActivityApplication.Services.Activity.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace ActivityApplication.Controllers.Activity;

public class ActivityController : BaseApiController
{
    private readonly IActivityService _service;
    private readonly ILogger<ActivityController> _logger;

    public ActivityController(IActivityService service, ILogger<ActivityController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet("GetActivities")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<ActivityDto>>> GetActivities()
    {
        try
        {
            return Ok(await _service.GetActivitiesAsync());
        }
        catch (IdNotFoundException e)
        {
            _logger.LogError("Error on getting the activity: " + e.Message);
            return NotFound();
        }
        catch (Exception e)
        {
            _logger.LogError("Error while getting activity: " + e.Message);
            return BadRequest();
        }
    }

    [HttpGet("GetActivity")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ActivityDto?>> GetActivity(Guid activityId)
    {
        try
        {
            return Ok(await _service.GetActivityAsync(activityId));
        }
        catch (IdNotFoundException e)
        {
            _logger.LogError("Error on getting the activity: " + e.Message);
            return NotFound();
        }
        catch (Exception e)
        {
            _logger.LogError("Error while getting activity: " + e.Message);
            return BadRequest();
        }
    }

    [HttpPost("Create")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ActivityDto?>> CreateActivity(ActivityDto activityDto)
    {
        try
        {
            if (!ModelState.IsValid)
                throw new Exception("Error in data's format!");

            return Ok(await _service.CreateActivityAsync(activityDto));
        }
        catch (DateTimeValidationException e)
        {
            _logger.LogError("Problem with saving date entered in the Date input: " + e.Message);
            return BadRequest();
        }
        catch (StringLengthException e)
        {
            _logger.LogError("Problem with saving inputs: " + e.Message);
            return BadRequest();
        }
        catch (Exception e)
        {
            _logger.LogError("Error while getting activity: " + e.Message);
            return BadRequest();
        }
    }

    [HttpPut("Update")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ActivityDto?>> UpdateActivity(
        Guid activityId,
        ActivityDto activityDto)
    {
        try
        {
            if (!ModelState.IsValid)
                throw new Exception("Error in data's format!");

            if (await _service.UpdateActivityAsync(activityId, activityDto))
                return Ok(await _service.GetActivityAsync(activityId));

            return BadRequest();
        }
        catch (IdNotFoundException e)
        {
            _logger.LogError("Error on getting the activity: " + e.Message);
            return NotFound();
        }
        catch (DateTimeValidationException e)
        {
            _logger.LogError("Problem with saving date entered in the Date input: " + e.Message);
            return BadRequest();
        }
        catch (StringLengthException e)
        {
            _logger.LogError("Problem with saving inputs: " + e.Message);
            return BadRequest();
        }
        catch (Exception e)
        {
            _logger.LogError("Error while getting activity: " + e.Message);
            return BadRequest();
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
            if (await _service.DeleteActivityAsync(activityId))
                return Ok("The activity has been deleted successfully!");

            return BadRequest("Error deleting the activity!");
        }
        catch (IdNotFoundException e)
        {
            _logger.LogError("Error on getting the activity: " + e.Message);
            return NotFound();
        }
        catch (Exception e)
        {
            _logger.LogError("Error while getting activity: " + e.Message);
            return BadRequest();
        }
    }
}