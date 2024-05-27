using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using JoinJoy.Core.Models;

namespace JoinJoy.Infrastructure.Configurations
{
    public class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.Content)
                   .IsRequired()
                   .HasMaxLength(1000);

            builder.Property(m => m.Timestamp)
                   .IsRequired();

            builder.Property(m => m.IsRead)
                   .IsRequired();

            builder.HasOne(m => m.Sender)
                   .WithMany(u => u.SentMessages) // Use SentMessages collection
                   .HasForeignKey(m => m.SenderId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.Receiver)
                   .WithMany(u => u.ReceivedMessages) // Use ReceivedMessages collection
                   .HasForeignKey(m => m.ReceiverId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
