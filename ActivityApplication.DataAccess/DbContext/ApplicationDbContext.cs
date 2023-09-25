using ActivityApplication.DataAccess.Activities;
using ActivityApplication.DataAccess.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ActivityApplication.DataAccess.DbContext;

public class ApplicationDbContext : IdentityDbContext<User, Role, Guid>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Activity?> Activities { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(GetType().Assembly);

        builder.Entity<Role>()
            .HasData(
                new Role { Id = Guid.NewGuid(), Name = "Admin", NormalizedName = "ADMIN" },
                new Role { Id = Guid.NewGuid(), Name = "User", NormalizedName = "USER" },
                new Role { Id = Guid.NewGuid(), Name = "Member", NormalizedName = "MEMBER" },
                new Role { Id = Guid.NewGuid(), Name = "BannedUser", NormalizedName = "BANNED_USER" },
                new Role { Id = Guid.NewGuid(), Name = "Moderator", NormalizedName = "MODERATOR" }
            );
    }
}