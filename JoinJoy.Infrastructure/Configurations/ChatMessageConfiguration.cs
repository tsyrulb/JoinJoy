using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using JoinJoy.Core.Models;

namespace JoinJoy.Infrastructure.Configurations
{
    public class ChatMessageConfiguration : IEntityTypeConfiguration<ChatMessage>
    {
        public void Configure(EntityTypeBuilder<ChatMessage> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.SenderId).IsRequired();
            builder.Property(c => c.ReceiverId).IsRequired();
            builder.Property(c => c.Message).IsRequired().HasMaxLength(2000);
        }
    }
}
