using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JoinJoy.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JoinJoy.Infrastructure.Configurations
{
    public class ChatMessageConfiguration : IEntityTypeConfiguration<ChatMessage>
    {
        public void Configure(EntityTypeBuilder<ChatMessage> builder)
        {
            builder.HasKey(cm => cm.Id);
            builder.Property(cm => cm.SenderId).IsRequired();
            builder.Property(cm => cm.ReceiverId).IsRequired();
            builder.Property(cm => cm.Message).IsRequired();
            builder.Property(cm => cm.Timestamp).IsRequired();
        }
    }
}
