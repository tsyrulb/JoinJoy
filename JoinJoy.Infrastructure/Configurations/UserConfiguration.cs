using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using JoinJoy.Core.Models;

namespace JoinJoy.Infrastructure.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.UserID);
            builder.Property(u => u.Username).IsRequired().HasMaxLength(100);
            builder.Property(u => u.Email).IsRequired().HasMaxLength(100);
            builder.Property(u => u.PasswordHash).IsRequired();
            builder.Property(u => u.Interests).HasMaxLength(2000);
            builder.Property(u => u.Hobbies).HasMaxLength(2000);
            builder.Property(u => u.Activities).HasMaxLength(2000);
            builder.Property(u => u.PreferredDestinations).HasMaxLength(2000);
            builder.Property(u => u.Availability).HasMaxLength(2000);
        }
    }
}
