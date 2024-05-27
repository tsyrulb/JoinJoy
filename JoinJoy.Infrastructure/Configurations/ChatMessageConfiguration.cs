using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using JoinJoy.Core.Models;

namespace JoinJoy.Infrastructure.Configurations
{
    public class ChatMessageConfiguration : IEntityTypeConfiguration<ChatMessage>
    {
        public void Configure(EntityTypeBuilder<ChatMessage> builder)
        {
            builder.HasKey(cm => cm.Id);

            builder.Property(cm => cm.Content)
                   .IsRequired()
                   .HasMaxLength(1000);

            builder.Property(cm => cm.Timestamp)
                   .IsRequired();

            builder.Property(cm => cm.IsRead)
                   .IsRequired();

            builder.HasOne(cm => cm.Sender)
                   .WithMany(u => u.SentChatMessages) // Correct collection for sent messages
                   .HasForeignKey(cm => cm.SenderId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(cm => cm.Receiver)
                   .WithMany(u => u.ReceivedChatMessages) // Correct collection for received messages
                   .HasForeignKey(cm => cm.ReceiverId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
