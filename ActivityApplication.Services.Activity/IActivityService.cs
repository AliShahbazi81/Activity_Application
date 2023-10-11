using ActivityApplication.Domain.Results;
using ActivityApplication.Services.Activity.DTO;

namespace ActivityApplication.Services.Activity;

public interface IActivityService
{
    /// <summary>
    /// Create an activity using Dto
    /// </summary>
    /// <param name="activityDto"></param>
    /// <returns></returns>
    Task<Result<ActivityDto>> CreateActivityAsync(ActivityDto activityDto, Guid userId);

    /// <summary>
    /// Get an Activity using activityId
    /// </summary>
    /// <param name="activityId"></param>
    /// <returns></returns>
    Task<Result<ActivityDto>> GetActivityAsync(Guid activityId);

    /// <summary>
    /// Get all of the available activities
    /// </summary>
    /// <returns></returns>
    Task<Result<IEnumerable<ActivityDto>>> GetActivitiesAsync();

    /// <summary>
    /// Update an activity
    /// </summary>
    /// <param name="activityId"></param>
    /// <param name="activityDto"></param>
    /// <returns></returns>
    Task<Result<bool>> UpdateActivityAsync(Guid activityId, ActivityDto activityDto);

    /// <summary>
    /// Delete an activity from database
    /// </summary>
    /// <param name="activityId"></param>
    /// <returns></returns>
    Task<Result<bool>> DeleteActivityAsync(Guid activityId);
}