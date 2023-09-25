using ActivityApplication.Services.Activity.DTO;

namespace ActivityApplication.Services.Activity.Services.Mapper;

public static class Mapper
{
    public static ActivityDto ToDto(DataAccess.Activities.Activity activityEntity)
    {
        return new ActivityDto
        {
            Title = activityEntity.Title,
            Category = activityEntity.Category,
            Date = Convert.ToDateTime(activityEntity.Date.ToLocalTime().ToString("MMMM YYYY")),
            Description = activityEntity.Description,
            City = activityEntity.City,
            Venue = activityEntity.Venue
        };
    }

    public static IEnumerable<ActivityDto>? ToDtos(IEnumerable<DataAccess.Activities.Activity>? activityEntities)
    {
        return activityEntities?.Select(dto => ToDto(dto));
    }

    public static DataAccess.Activities.Activity ToEntity(ActivityDto activityDto)
    {
        return new DataAccess.Activities.Activity
        {
            Title = activityDto.Title,
            Category = activityDto.Category,
            Date = Convert.ToDateTime(activityDto.Date.ToUniversalTime().ToString("MMMM YYYY")),
            Description = activityDto.Description,
            City = activityDto.City,
            Venue = activityDto.Venue
        };
    }

    public static IEnumerable<DataAccess.Activities.Activity> ToEntities(IEnumerable<ActivityDto>? activityDtos)
    {
        return activityDtos?.Select(dto => ToEntity(dto))!;
    }
}