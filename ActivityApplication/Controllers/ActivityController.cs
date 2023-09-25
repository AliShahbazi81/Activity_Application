using ActivityApplication.Services.Activity.DTO;
using ActivityApplication.Services.Activity.Services.Mapper;
using ActivityApplication.Services.Activity.Services.Mediator;
using Microsoft.AspNetCore.Mvc;

namespace ActivityApplication.Controllers.Activity;

public class ActivityController : BaseApiController
{
    private readonly ILogger<ActivityController> _logger;

    public ActivityController(ILogger<ActivityController> logger)
    {
        _logger = logger;
    }

    [HttpGet("GetActivity/{activityId}")]
    public async Task<ActivityDto> GetActivity(Guid activityId)
    {
        try
        {
            //TODO: Error handling
            var getActivity = await Mediator?.Send(new GetActivity.Query
                { Id = activityId })!;

            return Mapper.ToDto(getActivity);
        }
        catch (Exception e)
        {
            _logger.LogError("Error getting the activity: ", e.Message);
            throw;
        }
    }

    [HttpGet("GetActivities")]
    public async Task<IEnumerable<ActivityDto>?> GetActivities()
    {
        try
        {
            //TODO: Error handling
            var getActivities = await Mediator?.Send(new GetActivityList.Query())!;

            return Mapper.ToDtos(getActivities);
        }
        catch (Exception e)
        {
            _logger.LogError("Error on getting the activities: ", e.Message);
            throw;
        }
    }

    [HttpPost("CreateActivity")]
    public async Task<IActionResult> CreateActivity(ActivityDto activityDto)
    {
        try
        {
            //TODO: Error Handling
            var dtoToEntity = Mapper.ToEntity(activityDto);
            await Mediator?.Send(new CreateActivity.Command { Activity = dtoToEntity })!;

            return Ok();
        }
        catch (Exception e)
        {
            _logger.LogError("Error on creating the activity: ", e.Message);
            throw;
        }
    }
}