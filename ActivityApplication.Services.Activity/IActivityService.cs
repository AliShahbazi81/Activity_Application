using ActivityApplication.Services.Activity.DTO;

namespace ActivityApplication.Services.Activity;

public interface IActivityService
{
    /// <summary>
    /// Create an activity using Dto
    /// </summary>
    /// <param name="activityDto"></param>
    /// <returns></returns>
    Task<ActivityDto?> CreateActivityAsync(ActivityDto activityDto);

    /// <summary>
    /// Get an Activity using activityId
    /// </summary>
    /// <param name="activityId"></param>
    /// <returns></returns>
    Task<ActivityDto> GetActivityAsync(Guid activityId);

    /// <summary>
    /// Get all of the available activities
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<ActivityDto>> GetActivitiesAsync();

    /// <summary>
    /// Update an activity
    /// </summary>
    /// <param name="activityId"></param>
    /// <param name="activityDto"></param>
    /// <returns></returns>
    Task<bool> UpdateActivityAsync(
        Guid activityId,
        ActivityDto activityDto);

    /// <summary>
    /// Delete an activity from database
    /// </summary>
    /// <param name="activityId"></param>
    /// <returns></returns>
    Task<bool> DeleteActivityAsync(Guid activityId);
}