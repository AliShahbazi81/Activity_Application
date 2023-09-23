using System.Data;
using ActivityApplication.DataAccess.DbContext;
using ActivityApplication.Services.Activity.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ActivityApplication.Services.Activity;

public class ActivityService : IActivityService
{
    private readonly IDbContextFactory<ApplicationDbContext> _context;
    private readonly ILogger<ActivityService> _logger;

    public ActivityService(IDbContextFactory<ApplicationDbContext> context, ILogger<ActivityService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<ActivityDto?> CreateActivityAsync(ActivityDto activityDto)
    {
        await using var dbContext = await _context.CreateDbContextAsync();
        await using var transaction = await dbContext.Database.BeginTransactionAsync(IsolationLevel.Serializable);
        
        try
        {
            _ = dbContext.Add(new DataAccess.Activities.Activity
            {
                Title = activityDto.Title,
                Date = Convert.ToDateTime(activityDto.Date.ToUniversalTime().ToString("g")),
                Description = activityDto.Description,
                Category = activityDto.Category,
                City = activityDto.City,
                Venue = activityDto.Venue
            }).Entity;

            var saved = await dbContext.SaveChangesAsync() > 0;

            if (!saved)
                return null;

            return new ActivityDto
            {
                Title = activityDto.Title,
                Date = activityDto.Date,
                Description = activityDto.Description,
                Category = activityDto.Category,
                City = activityDto.City,
                Venue = activityDto.Venue,
                
            };
        }
        catch (Exception e)
        {
            _logger.LogInformation(e, "An error has occured during creating an Activity");
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<ActivityDto?> GetActivityAsync(Guid activityId)
    {
        await using var dbContext = await _context.CreateDbContextAsync();

        var getActivity = await dbContext.Activities
            .AsNoTracking()
            .Where(x => x.Id == activityId)
            .FirstOrDefaultAsync();

        if (getActivity != null)
            return new ActivityDto
            {
                Title = getActivity.Title,
                Date = getActivity.Date,
                Description = getActivity.Description,
                Category = getActivity.Category,
                City = getActivity.City,
                Venue = getActivity.Venue,
            };

        return null;
    }

    public async Task<IEnumerable<ActivityDto>> GetActivitiesAsync()
    {
        await using var dbContext = await _context.CreateDbContextAsync();
        var getActivities = await dbContext.Activities
            .AsNoTracking()
            .ToListAsync();

        var activityList = getActivities.Select(getActivity => new ActivityDto
        {
            Title = getActivity.Title,
            Date = getActivity.Date,
            Description = getActivity.Description,
            Category = getActivity.Category,
            City = getActivity.City,
            Venue = getActivity.Venue,
        });

        return activityList;
    }

    public async Task<bool> UpdateActivityAsync(
        Guid activityId, 
        ActivityDto activityDto)
    {
        await using var dbContext = await _context.CreateDbContextAsync();
        await using var transaction = await dbContext.Database.BeginTransactionAsync(IsolationLevel.Serializable);

        var getActivity = await dbContext.Activities
            .Where(x => x.Id == activityId)
            .SingleAsync();
        
        try
        {
            getActivity.Title = activityDto.Title;
            getActivity.Date = Convert.ToDateTime(activityDto.Date.ToUniversalTime().ToString("g"));
            getActivity.Description = activityDto.Description;
            getActivity.Category = activityDto.Category;
            getActivity.City = activityDto.City;
            getActivity.Venue = activityDto.Venue;

            var saved = await dbContext.SaveChangesAsync() > 0;

            if (!saved) return false;
            
            await transaction.CommitAsync();
            return true;

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            _logger.LogInformation(e, "An error occured during updating the activity");
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<bool> DeleteActivityAsync(Guid activityId)
    {
        await using var dbContext = await _context.CreateDbContextAsync();
        await using var transaction = await dbContext.Database.BeginTransactionAsync(IsolationLevel.Serializable);

        var getActivity = await dbContext.Activities
            .Where(x => x.Id == activityId)
            .SingleAsync();

        try
        {
            dbContext.Activities.Remove(getActivity);
            var saved = await dbContext.SaveChangesAsync() > 0;

            if (!saved) return false;
            
            await transaction.CommitAsync();
            return true;

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            _logger.LogInformation(e, "An error occured when Deleting the activity");
            await transaction.RollbackAsync();
            throw;
        }
    }
}