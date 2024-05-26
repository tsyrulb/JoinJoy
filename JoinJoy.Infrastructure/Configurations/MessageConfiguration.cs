using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using JoinJoy.Core.Models;

namespace JoinJoy.Infrastructure.Configurations
{
    public class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.HasKey(m => m.MessageID);
            builder.Property(m => m.SenderID).IsRequired();
            builder.Property(m => m.ReceiverID).IsRequired();
            builder.Property(m => m.Content).IsRequired().HasMaxLength(2000);
        }
    }
}
