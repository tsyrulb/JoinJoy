using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using JoinJoy.Core.Models;

namespace JoinJoy.Infrastructure.Configurations
{
    public class ActivityConfiguration : IEntityTypeConfiguration<Activity>
    {
        public void Configure(EntityTypeBuilder<Activity> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Name).IsRequired();
            builder.Property(e => e.LocationId).IsRequired();

            builder.HasOne(d => d.CreatedBy)
                   .WithMany(p => p.CreatedActivities)
                   .HasForeignKey(d => d.CreatedById)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(d => d.Location)
                   .WithMany(l => l.Activities)
                   .HasForeignKey(d => d.LocationId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(e => e.UserActivities)
                   .WithOne(ua => ua.Activity)
                   .HasForeignKey(ua => ua.ActivityId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(e => e.Matches)
                   .WithOne(m => m.Activity)
                   .HasForeignKey(m => m.ActivityId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

