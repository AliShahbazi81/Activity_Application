using System.Data;
using ActivityApplication.DataAccess.DbContext;
using ActivityApplication.Services.Activity.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ActivityApplication.Services.Activity;

public class ActivityService : IActivityService
{
    private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
    private readonly ILogger<ActivityService> _logger;

    public ActivityService(IDbContextFactory<ApplicationDbContext> context, ILogger<ActivityService> logger)
    {
        _contextFactory = context;
        _logger = logger;
    }

    public async Task<ActivityDto?> CreateActivityAsync(ActivityDto activityDto)
    {
        return await ExecuteInTransactionAsync<ActivityDto?>(async dbContext =>
        {
            var activity = MapToEntity(activityDto);
            dbContext.Add(activity);
            return await dbContext.SaveChangesAsync() > 0 ? activityDto : null;
        }, "An error has occurred during creating an Activity");
    }

        public async Task<ActivityDto?> GetActivityAsync(Guid activityId)
        {
            await using var dbContext = await _contextFactory.CreateDbContextAsync();
            
            var activity = await dbContext.Activities
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == activityId);
            
            return activity != null ? MapToDto(activity) : null;
        }

        public async Task<IEnumerable<ActivityDto>> GetActivitiesAsync()
        {
            await using var dbContext = await _contextFactory.CreateDbContextAsync();
            var activities = await dbContext.Activities
                .AsNoTracking()
                .ToListAsync();
            return activities.Select(MapToDto);
        }

        public async Task<bool> UpdateActivityAsync(Guid activityId, ActivityDto activityDto)
        {
            return await ExecuteInTransactionAsync(async dbContext =>
            {
                var activity = await dbContext.Activities
                    .SingleAsync(x => x.Id == activityId);
                
                UpdateEntity(activity, activityDto);
                
                return await dbContext.SaveChangesAsync() > 0;
            }, "An error occurred during updating the activity");
        }

        public async Task<bool> DeleteActivityAsync(Guid activityId)
        {
            return await ExecuteInTransactionAsync(async dbContext =>
            {
                var activity = await dbContext.Activities
                    .SingleAsync(x => x.Id == activityId);
                
                dbContext.Activities.Remove(activity);
                
                return await dbContext.SaveChangesAsync() > 0;
            }, "An error occurred when deleting the activity");
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
        
        private static ActivityDto MapToDto(DataAccess.Activities.Activity activity)
        {
            return new ActivityDto
            {
                Title = activity.Title,
                Date = activity.Date,
                Description = activity.Description,
                Category = activity.Category,
                City = activity.City,
                Venue = activity.Venue,
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
                Venue = activityDto.Venue,
            };
        }

        private static void UpdateEntity(
            DataAccess.Activities.Activity activity, 
            ActivityDto activityDto)
        {
            activity.Title = activityDto.Title;
            activity.Date = Convert.ToDateTime(activityDto.Date.ToUniversalTime().ToString("g"));
            activity.Description = activityDto.Description;
            activity.Category = activityDto.Category;
            activity.City = activityDto.City;
            activity.Venue = activityDto.Venue;
        }

}