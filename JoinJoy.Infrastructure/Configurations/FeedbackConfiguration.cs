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
    public class FeedbackConfiguration : IEntityTypeConfiguration<Feedback>
    {
        public void Configure(EntityTypeBuilder<Feedback> builder)
        {
            builder.Property(f => f.Rating).IsRequired();
            builder.Property(f => f.Comments).HasMaxLength(1000);
            builder.Property(f => f.FeedbackDate).IsRequired();
            // Other configurations for Feedback entity
        }
    }
}
