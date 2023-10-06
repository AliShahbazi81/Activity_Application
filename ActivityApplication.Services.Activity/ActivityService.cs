using System.Data;
using ActivityApplication.DataAccess.DbContext;
using ActivityApplication.Domain.Results;
using ActivityApplication.Services.Activity.DTO;
using ActivityApplication.Services.Activity.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ActivityApplication.Services.Activity;

public class ActivityService : IActivityService
{
    private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
    private readonly ILogger<ActivityService> _logger;
    private const int TitleLength = 5;
    private const int CityLength = 5;
    private const int CategoryLength = 5;
    private const int VenueLength = 5;
    private const int DescriptionLength = 10;

    // Comment
    public ActivityService(IDbContextFactory<ApplicationDbContext> context, ILogger<ActivityService> logger)
    {
        _contextFactory = context;
        _logger = logger;
    }

    public async Task<Result<ActivityDto>> CreateActivityAsync(ActivityDto activityDto)
    {
        // Validate the input
        InputValidation(activityDto.Date,
            activityDto.Title,
            activityDto.Category,
            activityDto.City,
            activityDto.Venue,
            activityDto.Description);

        // Convert the DTO to an entity
        var activity = MapToEntity(activityDto);

        // Using a new DB context
        await using var dbContext = await _contextFactory.CreateDbContextAsync();
        dbContext.Add(activity);

        // Save and check if successful
        await dbContext.SaveChangesAsync();

        var returnedActivityDto = MapToDto(activity);
        return Result<ActivityDto>.Success(returnedActivityDto);
    }

    public async Task<Result<ActivityDto>> GetActivityAsync(Guid activityId)
    {

        var activity = await CheckActivityId(activityId);

        return Result<ActivityDto>.Success(MapToDto(activity));
    }

    public async Task<Result<IEnumerable<ActivityDto>>> GetActivitiesAsync()
    {
        await using var dbContext = await _contextFactory.CreateDbContextAsync();
        var activities = await dbContext.Activities
            .AsNoTracking()
            .ToListAsync();

        return Result<IEnumerable<ActivityDto>>.Success(activities.Select(MapToDto));
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

        // Check and get the activity
        var activity = await CheckActivityId(activityId);

        // Update the entity with the provided DTO
        UpdateEntity(activity, activityDto);

        // Save changes and return success status
        return await dbContext.SaveChangesAsync() > 0 ? Result<bool>.Success(true) : Result<bool>.Failure("Failed to update the activity.");
    }


    public async Task<Result<bool>> DeleteActivityAsync(Guid activityId)
    {
        // Using a new DB context
        await using var dbContext = await _contextFactory.CreateDbContextAsync();

        // Check and get the activity
        var activity = await CheckActivityId(activityId);

        // Remove the activity
        dbContext.Activities.Remove(activity);

        // Save changes and return success status
        return await dbContext.SaveChangesAsync() > 0 ? Result<bool>.Success(true) : Result<bool>.Failure("Failed to delete the activity.");
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

    private static ActivityDto MapToDto(DataAccess.Activities.Activity? activity)
    {
        return new ActivityDto
        {
            Id = activity.Id,
            Title = activity.Title,
            Date = Convert.ToDateTime(activity.Date.ToLocalTime().ToString("g")),
            Description = activity.Description,
            Category = activity.Category,
            City = activity.City,
            Venue = activity.Venue
        };
    }

    private static DataAccess.Activities.Activity MapToEntity(ActivityDto activityDto)
    {
        return new DataAccess.Activities.Activity
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
        DataAccess.Activities.Activity? activity,
        ActivityDto activityDto)
    {
        activity.Title = activityDto.Title;
        activity.Date = Convert.ToDateTime(activityDto.Date.ToUniversalTime().ToString("g"));
        activity.Description = activityDto.Description;
        activity.Category = activityDto.Category;
        activity.City = activityDto.City;
        activity.Venue = activityDto.Venue;
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

    private async Task<DataAccess.Activities.Activity?> CheckActivityId(Guid activityId)
    {
        await using var dbContext = await _contextFactory.CreateDbContextAsync();

        var getActivity = dbContext.Activities
            .Where(x => x.Id == activityId);

        if (!await getActivity.AnyAsync())
            throw new IdNotFoundException();

        return await getActivity.SingleAsync();
    }
}