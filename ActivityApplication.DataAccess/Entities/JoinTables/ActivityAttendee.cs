using ActivityApplication.DataAccess.Entities.Activities;
using ActivityApplication.DataAccess.Entities.Users;

namespace ActivityApplication.DataAccess.Entities.JoinTables;

public class ActivityAttendee
{
    public bool IsHost { get; set; } = false;

    public Guid ActivityId { get; set; }
    public Activity Activity { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; }
}