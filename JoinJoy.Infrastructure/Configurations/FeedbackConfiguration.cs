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

            // Configure rating to be required and within a valid range (1 to 5)
            builder.Property(f => f.Rating)
                   .IsRequired()
                   .HasColumnType("int")
                   .HasDefaultValue(1);

            builder.Property(f => f.Timestamp)
                   .IsRequired();

            // Relationship with User (who provides the feedback)
            builder.HasOne(f => f.User)
                   .WithMany(u => u.Feedbacks)
                   .HasForeignKey(f => f.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Relationship with Activity (activity being reviewed)
            builder.HasOne(f => f.Activity)
                   .WithMany() // Assuming Activity does not have a Feedbacks collection
                   .HasForeignKey(f => f.ActivityId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Additional relationship with TargetUser (user receiving the feedback)
            builder.HasOne(f => f.TargetUser)
                   .WithMany()
                   .HasForeignKey(f => f.TargetUserId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

