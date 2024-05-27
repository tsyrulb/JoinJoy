using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using JoinJoy.Core.Models;

namespace JoinJoy.Infrastructure.Configurations
{
    public class ActivityConfiguration : IEntityTypeConfiguration<Activity>
    {
        public void Configure(EntityTypeBuilder<Activity> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(a => a.Description)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(a => a.Date)
                   .IsRequired();

            builder.Property(a => a.Location)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.HasOne(a => a.CreatedBy)
                   .WithMany(u => u.CreatedActivities) // Fix: Use a separate collection for created activities
                   .HasForeignKey(a => a.CreatedById)
                   .OnDelete(DeleteBehavior.Restrict);

            // Correctly define the many-to-many relationship between User and Activity
            builder.HasMany(a => a.UserActivities)
                   .WithOne(ua => ua.Activity)
                   .HasForeignKey(ua => ua.ActivityId);
        }
    }
}
