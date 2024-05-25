using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using JoinJoy.Core.Models;

namespace JoinJoy.Infrastructure
{
    public class JoinJoyDbContext : DbContext
    {
        public JoinJoyDbContext(DbContextOptions<JoinJoyDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
    }
}