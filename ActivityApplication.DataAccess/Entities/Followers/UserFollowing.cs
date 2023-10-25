using ActivityApplication.DataAccess.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ActivityApplication.DataAccess.Entities.Followers;

public class UserFollowing
{
    public Guid ObserverId { get; set; }
    public Guid TargetId { get; set; }
    public virtual User Observer { get; set; }
    public virtual User Target { get; set; }
}

public class UserFollowingBuildConfiguration : IEntityTypeConfiguration<UserFollowing>
{
    public void Configure(EntityTypeBuilder<UserFollowing> builder)
    {
        builder.HasKey(k => new { k.ObserverId, k.TargetId });

        builder.HasOne(x => x.Observer)
            .WithMany(x => x.Followings)
            .HasForeignKey(x => x.ObserverId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Target)
            .WithMany(x => x.Followers)
            .HasForeignKey(x => x.TargetId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}