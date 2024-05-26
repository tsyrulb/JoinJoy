using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using JoinJoy.Core.Models;

namespace JoinJoy.Infrastructure.Configurations
{
    public class ActivityConfiguration : IEntityTypeConfiguration<Activity>
    {
        public void Configure(EntityTypeBuilder<Activity> builder)
        {
            builder.HasKey(a => a.ActivityID);
            builder.Property(a => a.ActivityName).IsRequired().HasMaxLength(200);
            builder.Property(a => a.Description).HasMaxLength(2000);
            builder.Property(a => a.Category).HasMaxLength(100);
        }
    }
}
