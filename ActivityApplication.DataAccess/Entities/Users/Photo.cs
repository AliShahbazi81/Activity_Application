using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ActivityApplication.DataAccess.Entities.Users;

public class Photo
{
    public string PublicId { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public bool IsMain { get; set; } = false;

    // Each photo is only for 1 User at a time
    public Guid UserId { get; set; }
    public virtual User User { get; set; }
}

public class PhotoTypeConfiguration : IEntityTypeConfiguration<Photo>
{
    public void Configure(EntityTypeBuilder<Photo> builder)
    {
        // Make the Id as Primary key
        builder.HasKey(x => x.PublicId);

        builder.Property(x => x.PublicId)
            .IsRequired();
        builder.Property(x => x.Url)
            .IsRequired();
        builder.HasOne(u => u.User)
            .WithMany(p => p.Photos)
            .HasForeignKey(f => f.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}