using ActivityApplication.Services.Activity;
using ActivityApplication.Services.Activity.DTO;
using ActivityApplication.Services.Activity.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ActivityApplication.Controllers.ActivityControllers;

[Authorize]
public class ActivityController : BaseApiController
{
    private readonly ILogger<ActivityController> _logger;
    private readonly IActivityService _service;

    public ActivityController(ILogger<ActivityController> logger, IActivityService service)
    {
        _logger = logger;
        _service = service;
    }

    [HttpGet("Get/{activityId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetActivity(Guid activityId)
    {
        try
        {
            return HandleResult(await _service.GetActivityAsync(activityId));
        }
        catch (IdNotFoundException e)
        {
            _logger.LogError("Error getting the activity! ", e.Message);
            return NotFound();
        }
        catch (Exception e)
        {
            _logger.LogError("Error getting the activity: ", e.Message);
            throw;
        }
    }

    [HttpGet("Get")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetActivities()
    {
        try
        {
            return HandleResult(await _service.GetActivitiesAsync());
        }
        catch (Exception e)
        {
            _logger.LogError("Error on getting the activities: ", e.Message);
            return BadRequest();
        }
    }

    [HttpPost("Create")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateActivity(ActivityDto activityDto)
    {
        try
        {
            return HandleResult(await _service.CreateActivityAsync(activityDto));
        }
        catch (DateTimeValidationException e)
        {
            _logger.LogError("Date input validation error! ", e.Message);
            return BadRequest("Date input validation error!");
        }
        catch (StringLengthException e)
        {
            _logger.LogError("Error on string length!", e.Message);
            return BadRequest("The length of the input is less than minimum");
        }
        catch (Exception e)
        {
            _logger.LogError("Error on creating the activity: ", e.Message);
            return BadRequest();
        }
    }

    [HttpPost("{id}/attend")]
    public async Task<IActionResult> Attend(Guid id)
    {
        try
        {
            return HandleResult(await _service.UpdateAttendeesAsync(id));
        }
        catch (Exception e)
        {
            _logger.LogError("Error on attending the activity: ", e.Message);
            return BadRequest();
        }
    }

    [Authorize(Policy = "IsActivityHost")]
    [HttpPut("Update/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateActivity(Guid id, ActivityDto activityDto)
    {
        try
        {
            return HandleResult(await _service.UpdateActivityAsync(id, activityDto));
        }
        catch (IdNotFoundException e)
        {
            _logger.LogError("Id not found! ", e.Message);
            return NotFound();
        }
        catch (DateTimeValidationException e)
        {
            _logger.LogError("The chosen date has passed!", e.Message);
            return BadRequest("The chosen date has passed!");
        }
        catch (StringLengthException e)
        {
            _logger.LogError("Error on string length!", e.Message);
            return BadRequest("The length of the input is less than minimum");
        }
        catch (Exception e)
        {
            _logger.LogError("Error on creating the activity: ", e.Message);
            return BadRequest();
        }
    }

    [Authorize(Policy = "IsActivityHost")]
    [HttpDelete("Delete/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteActivity(Guid id)
    {
        try
        {
            return HandleResult(await _service.DeleteActivityAsync(id));
        }
        catch (IdNotFoundException e)
        {
            _logger.LogError("Id cannot be found! ", e.Message);
            return NotFound();
        }
        catch (Exception e)
        {
            _logger.LogError("Error on deleting the activity!", e.Message);
            return BadRequest();
        }
    }
}