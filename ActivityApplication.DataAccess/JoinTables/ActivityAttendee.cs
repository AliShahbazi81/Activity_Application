using ActivityApplication.DataAccess.Activities;
using ActivityApplication.DataAccess.Users;

namespace ActivityApplication.DataAccess.JoinTables;

public class ActivityAttendee
{
    public bool IsHost { get; set; } = false;

    public Guid ActivityId { get; set; }
    public Activity Activity { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; }
}