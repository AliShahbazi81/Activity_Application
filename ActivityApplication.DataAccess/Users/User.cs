using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ActivityApplication.DataAccess.Users;

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
}

public class UserBuilder : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
    }
}