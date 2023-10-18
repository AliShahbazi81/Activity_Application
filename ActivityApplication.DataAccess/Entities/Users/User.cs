using ActivityApplication.DataAccess.Entities.JoinTables;
using Microsoft.AspNetCore.Identity;

namespace ActivityApplication.DataAccess.Entities.Users;

public class User : IdentityUser<Guid>
{
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public bool IsDeleted { get; set; } = false;
    public string DisplayName { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
    public string DeletionReason { get; set; } = string.Empty;
    public DateTime? DeletionTime { get; set; } = null;
    public string DeletedBy { get; set; } = string.Empty;

    public virtual ICollection<ActivityAttendee> Activities { get; set; }

    // Each user can upload 1 or many images
    public virtual ICollection<Photo> Photos { get; set; }
}