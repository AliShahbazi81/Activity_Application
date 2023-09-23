using ActivityApplication.Services.Activity.DTO;

namespace ActivityApplication.Services.Activity;

public interface IActivityService
{
    Task<ActivityDto?> CreateActivityAsync(ActivityDto activityDto);

    Task<ActivityDto?> GetActivityAsync(Guid activityId);

    Task<IEnumerable<ActivityDto>> GetActivitiesAsync();

    Task<bool> UpdateActivityAsync(
        Guid activityId,
        ActivityDto activityDto);

    Task<bool> DeleteActivityAsync(Guid activityId);
}