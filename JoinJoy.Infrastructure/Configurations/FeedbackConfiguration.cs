using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using JoinJoy.Core.Models;

namespace JoinJoy.Infrastructure.Configurations
{
    public class FeedbackConfiguration : IEntityTypeConfiguration<Feedback>
    {
        public void Configure(EntityTypeBuilder<Feedback> builder)
        {
            builder.HasKey(f => f.Id);

            builder.Property(f => f.Comments)
                   .IsRequired()
                   .HasMaxLength(1000);

            builder.Property(f => f.Rating)
                   .IsRequired();

            builder.Property(f => f.Timestamp)
                   .IsRequired();

            builder.HasOne(f => f.User)
                   .WithMany(u => u.Feedbacks)
                   .HasForeignKey(f => f.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(f => f.Activity)
                   .WithMany() // Assuming Activity does not have a Feedbacks collection
                   .HasForeignKey(f => f.ActivityId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
