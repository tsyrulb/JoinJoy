using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using JoinJoy.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using JoinJoy.Core.Models;

namespace JoinJoy.Infrastructure.Configurations
{
    public class MatchConfiguration : IEntityTypeConfiguration<Match>
    {
        public void Configure(EntityTypeBuilder<Match> builder)
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.MatchDate)
                   .IsRequired();

            builder.Property(m => m.IsAccepted)
                   .IsRequired();

            builder.HasOne(m => m.User1)
                   .WithMany(u => u.Matches)
                   .HasForeignKey(m => m.UserId1)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.User2)
                   .WithMany()
                   .HasForeignKey(m => m.UserId2)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.Activity)
                   .WithMany(a => a.Matches)
                   .HasForeignKey(m => m.ActivityId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
