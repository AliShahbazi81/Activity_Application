using ActivityApplication.Services.Activity.DTO;

namespace ActivityApplication.Services.Activity.Services.Mapper;

public static class Mapper
{
    public static ActivityDto ToDto(DataAccess.Entities.Activities.Activity activityEntity)
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

    public static IEnumerable<ActivityDto>? ToDtos(IEnumerable<DataAccess.Entities.Activities.Activity>? activityEntities)
    {
        return activityEntities?.Select(dto => ToDto(dto));
    }

    public static DataAccess.Entities.Activities.Activity ToEntity(ActivityDto activityDto)
    {
        return new DataAccess.Entities.Activities.Activity
        {
            Title = activityDto.Title,
            Category = activityDto.Category,
            Date = Convert.ToDateTime(activityDto.Date.ToUniversalTime().ToString("MMMM YYYY")),
            Description = activityDto.Description,
            City = activityDto.City,
            Venue = activityDto.Venue
        };
    }

    public static IEnumerable<DataAccess.Entities.Activities.Activity> ToEntities(IEnumerable<ActivityDto>? activityDtos)
    {
        return activityDtos?.Select(dto => ToEntity(dto))!;
    }
}