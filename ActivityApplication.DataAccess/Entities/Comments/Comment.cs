using ActivityApplication.DataAccess.Entities.Activities;
using ActivityApplication.DataAccess.Entities.Users;

namespace ActivityApplication.DataAccess.Entities.Comments;

public class Comment
{
    public int Id { get; set; }
    public string Body { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public User Author { get; set; }
    public Activity Activity { get; set; }
}