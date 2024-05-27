using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using JoinJoy.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using JoinJoy.Core.Models;

namespace JoinJoy.Infrastructure.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(u => u.Email)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(u => u.Password)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(u => u.ProfilePhoto)
                   .HasMaxLength(200);

            builder.HasMany(u => u.UserActivities)
                   .WithOne(ua => ua.User)
                   .HasForeignKey(ua => ua.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(u => u.Matches)
                   .WithOne(m => m.User1)
                   .HasForeignKey(m => m.UserId1)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(u => u.Feedbacks)
                   .WithOne(f => f.User)
                   .HasForeignKey(f => f.UserId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
