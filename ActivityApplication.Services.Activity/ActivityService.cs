using System.Data;
using ActivityApplication.DataAccess.DbContext;
using ActivityApplication.DataAccess.Entities.JoinTables;
using ActivityApplication.Domain.Results;
using ActivityApplication.Infrastructure.Security;
using ActivityApplication.Services.Activity.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ActivityApplication.Services.Activity;

public class ActivityService : IActivityService
{
    private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
    private readonly ILogger<ActivityService> _logger;
    private readonly IUserAccessor _userAccessor;
    private const int TitleLength = 5;
    private const int CityLength = 5;
    private const int CategoryLength = 5;
    private const int VenueLength = 5;
    private const int DescriptionLength = 10;

    // Comment
    public ActivityService(IDbContextFactory<ApplicationDbContext> context, ILogger<ActivityService> logger, IUserAccessor userAccessor)
    {
        _contextFactory = context;
        _logger = logger;
        _userAccessor = userAccessor;
    }

    public async Task<Result<ActivityDto>> CreateActivityAsync(ActivityDto activityDto)
    {
        // Validate the input
        /*InputValidation(activityDto.Date,
            activityDto.Title,
            activityDto.Category,
            activityDto.City,
            activityDto.Venue,
            activityDto.Description);*/

        // Convert the DTO to an entity
        var activity = MapToEntity(activityDto);

        // Using a new DB context
        await using var dbContext = await _contextFactory.CreateDbContextAsync();
        dbContext.Add(activity);

        // Add the created activity to the relational DbSet with Attendees
        var attendee = new ActivityAttendee
        {
            UserId = _userAccessor.GetUserId(),
            Activity = activity,
            ActivityId = activity.Id,
            IsHost = true
        };

        dbContext.Add(attendee);

        // Save and check if successful
        await dbContext.SaveChangesAsync();

        // Re-query the database to get fully loaded activity entity
        var savedActivity = await dbContext.Activities
            .Include(a => a.Attendees)
            .ThenInclude(aa => aa.User)
            .SingleOrDefaultAsync(a => a.Id == activity.Id);

        var returnedActivityDto = MapToDto(savedActivity);
        return Result<ActivityDto>.Success(returnedActivityDto);
    }

    public async Task<Result<ActivityDto>> GetActivityAsync(Guid activityId)
    {
        var dbContext = await _contextFactory.CreateDbContextAsync();

        var activity = await CheckActivityId(dbContext, activityId);

        var getDto = MapToDto(activity);

        return Result<ActivityDto>.Success(getDto);
    }

    public async Task<Result<IEnumerable<ActivityDto>>> GetActivitiesAsync()
    {
        await using var dbContext = await _contextFactory.CreateDbContextAsync();
        var activities = await dbContext.Activities
            .AsNoTracking()
            .Include(a => a.Attendees)
            .ThenInclude(aa => aa.User)
            .ToListAsync();

        var activityDtos = activities.Select(MapToDto);

        return Result<IEnumerable<ActivityDto>>.Success(activityDtos);
    }

    public async Task<Result<bool>> UpdateActivityAsync(Guid activityId, ActivityDto activityDto)
    {
        // Validate the input
        InputValidation(activityDto.Date,
            activityDto.Title,
            activityDto.Category,
            activityDto.City,
            activityDto.Venue,
            activityDto.Description);

        // Using a new DB context
        await using var dbContext = await _contextFactory.CreateDbContextAsync();

        var activity = await dbContext.Activities
            .Where(x => x.Id == activityId)
            .SingleOrDefaultAsync();

        // Check and get the activity
        if (activity == null)
            return Result<bool>.Failure("Activity not found.");

        // Update the entity with the provided DTO
        UpdateEntity(activity, activityDto);

        // Save changes and return success status
        var saved = await dbContext.SaveChangesAsync() > 0;

        return saved ? Result<bool>.Success(true) : Result<bool>.Failure("Failed to update the activity.");
    }


    public async Task<Result<bool>> DeleteActivityAsync(Guid activityId)
    {
        // Using a new DB context
        await using var dbContext = await _contextFactory.CreateDbContextAsync();

        // Check and get the activity
        var activity = await CheckActivityId(dbContext, activityId);

        // Remove the activity
        dbContext.Activities.Remove(activity);

        // Save changes and return success status
        return await dbContext.SaveChangesAsync() > 0 ? Result<bool>.Success(true) : Result<bool>.Failure("Failed to delete the activity.");
    }

    public async Task<Result<bool>> UpdateAttendeesAsync(Guid activityId)
    {
        // Create dbContext
        var dbContext = await _contextFactory.CreateDbContextAsync();

        // Get desired activity
        var activity = await dbContext.Activities
            .Include(a => a.Attendees)
            .ThenInclude(u => u.User)
            .SingleOrDefaultAsync(x => x.Id == activityId);

        // Check if the activity exists or not
        if (activity == null) return Result<bool>.Failure("Failed to fetch the activity!");

        // Get The user who wants to participate/cancel participation
        var user = dbContext.Users
            .FirstOrDefault(x => x.UserName == _userAccessor.GetUserUsername());

        // Check if user exists or not
        if (user == null) return Result<bool>.Failure("Failed to fetch the user!");

        // Get the Host Username
        var hostUsername = activity.Attendees
            .FirstOrDefault(x => x.IsHost)?.User.UserName;

        // Get the attendees
        var attendance = activity.Attendees
            .FirstOrDefault(x => x.User.UserName == user.UserName);

        // Check if attendance is not null
        if (attendance != null && hostUsername == user.UserName)
            activity.IsCanceled = !activity.IsCanceled;

        if (attendance != null && hostUsername != user.UserName)
            activity.Attendees.Remove(attendance);

        if (attendance == null)
        {
            attendance = new ActivityAttendee
            {
                User = user,
                Activity = activity,
                IsHost = false
            };
            activity.Attendees.Add(attendance);
        }

        // Save to database
        var result = await dbContext.SaveChangesAsync() > 0;

        // Return the result 
        return result ? Result<bool>.Success(true) : Result<bool>.Failure("Action failed!");
    }

    private async Task<TResult?> ExecuteInTransactionAsync<TResult>(
        Func<ApplicationDbContext,
            Task<TResult?>> action,
        string errorMessage)
    {
        await using var dbContext = await _contextFactory.CreateDbContextAsync();
        await using var transaction = await dbContext.Database.BeginTransactionAsync(IsolationLevel.Serializable);
        try
        {
            var result = await action(dbContext);
            await transaction.CommitAsync();
            return result;
        }
        catch (Exception e)
        {
            _logger.LogInformation(e, errorMessage);
            await transaction.RollbackAsync();
            throw;
        }
    }

    private ActivityDto MapToDto(DataAccess.Entities.Activities.Activity? activity)
    {
        var profiles = activity.Attendees.Select(a => MapToProfileDto(a.User)).ToList();

        var hostUserName = activity.Attendees
            .Select(x => x.User.UserName)
            .FirstOrDefault();

        return new ActivityDto
        {
            Id = activity.Id,
            Title = activity.Title,
            Date = Convert.ToDateTime(activity.Date.ToLocalTime().ToString("g")),
            Description = activity.Description,
            Category = activity.Category,
            City = activity.City,
            Venue = activity.Venue,
            HostUsername = hostUserName,
            Attendees = profiles
        };
    }

    private ProfileDto MapToProfileDto(DataAccess.Entities.Users.User attendee)
    {
        return new ProfileDto
        {
            Username = attendee.UserName,
            DisplayName = attendee.DisplayName,
            Bio = attendee.Bio
        };
    }

    private static DataAccess.Entities.Activities.Activity MapToEntity(ActivityDto activityDto)
    {
        return new DataAccess.Entities.Activities.Activity
        {
            Title = activityDto.Title,
            Date = Convert.ToDateTime(activityDto.Date.ToUniversalTime().ToString("g")),
            Description = activityDto.Description,
            Category = activityDto.Category,
            City = activityDto.City,
            Venue = activityDto.Venue
        };
    }

    private static void UpdateEntity(
        DataAccess.Entities.Activities.Activity? activity,
        ActivityDto activityDto)
    {
        activity.Title = activityDto.Title;
        activity.Date = Convert.ToDateTime(activityDto.Date.ToString("g"));
        activity.Description = activityDto.Description;
        activity.Category = activityDto.Category;
        activity.City = activityDto.City;
        activity.Venue = activityDto.Venue;
        activity.IsCanceled = activityDto.IsCanceled;
    }

    private static void InputValidation(
        DateTime date,
        string title,
        string category,
        string city,
        string venue,
        string description)
    {
        Guards.ActivityGuard.CheckDate(date);
        Guards.ActivityGuard.CheckLength(title, TitleLength);
        Guards.ActivityGuard.CheckLength(category, CategoryLength);
        Guards.ActivityGuard.CheckLength(city, CityLength);
        Guards.ActivityGuard.CheckLength(venue, VenueLength);
        Guards.ActivityGuard.CheckLength(description, DescriptionLength);
    }

    private async Task<DataAccess.Entities.Activities.Activity?> CheckActivityId(ApplicationDbContext dbContext, Guid activityId)
    {
        var getActivity = await dbContext.Activities
            .Where(x => x.Id == activityId)
            .SingleOrDefaultAsync();

        return getActivity ?? null;
    }

    private async Task<IEnumerable<DataAccess.Entities.Users.User>> AttendeesListAsync(Guid activityId)
    {
        await using var dbContext = await _contextFactory.CreateDbContextAsync();
        var getList = await dbContext.ActivityAttendees
            .AsNoTracking()
            .Where(x => x.ActivityId == activityId)
            .Select(x => x.User)
            .ToListAsync();

        return getList;
    }
}