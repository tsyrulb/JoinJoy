using Microsoft.EntityFrameworkCore;
using JoinJoy.Core.Models;
using JoinJoy.Infrastructure.Configurations;

namespace JoinJoy.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<UserActivity> UserActivities { get; set; }
        public DbSet<PreferredDestination> PreferredDestinations { get; set; }
        public DbSet<UserPreferredDestination> UserPreferredDestinations { get; set; }
        public DbSet<Availability> Availabilities { get; set; }
        public DbSet<UserAvailability> UserAvailabilities { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Subcategory> Subcategories { get; set; }
        public DbSet<UserSubcategory> UserSubcategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply all configurations
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new ActivityConfiguration());
            modelBuilder.ApplyConfiguration(new ChatMessageConfiguration());
            modelBuilder.ApplyConfiguration(new FeedbackConfiguration());
            modelBuilder.ApplyConfiguration(new MatchConfiguration());
            modelBuilder.ApplyConfiguration(new MessageConfiguration());

            // Configure many-to-many relationships
            modelBuilder.Entity<UserActivity>()
                .HasKey(ua => new { ua.UserId, ua.ActivityId });

            modelBuilder.Entity<UserActivity>()
                .HasOne(ua => ua.User)
                .WithMany(u => u.UserActivities)
                .HasForeignKey(ua => ua.UserId);

            modelBuilder.Entity<UserActivity>()
                .HasOne(ua => ua.Activity)
                .WithMany(a => a.UserActivities)
                .HasForeignKey(ua => ua.ActivityId);

            modelBuilder.Entity<UserPreferredDestination>()
                .HasKey(upd => new { upd.UserId, upd.PreferredDestinationId });

            modelBuilder.Entity<UserPreferredDestination>()
                .HasOne(upd => upd.User)
                .WithMany(u => u.UserPreferredDestinations)
                .HasForeignKey(upd => upd.UserId);

            modelBuilder.Entity<UserPreferredDestination>()
                .HasOne(upd => upd.PreferredDestination)
                .WithMany(pd => pd.UserPreferredDestinations)
                .HasForeignKey(upd => upd.PreferredDestinationId);

            modelBuilder.Entity<UserAvailability>()
                .HasKey(ua => new { ua.UserId, ua.AvailabilityId });

            modelBuilder.Entity<UserAvailability>()
                .HasOne(ua => ua.User)
                .WithMany(u => u.UserAvailabilities)
                .HasForeignKey(ua => ua.UserId);

            modelBuilder.Entity<UserAvailability>()
                .HasOne(ua => ua.Availability)
                .WithMany(a => a.UserAvailabilities)
                .HasForeignKey(ua => ua.AvailabilityId);

            modelBuilder.Entity<UserSubcategory>()
                .HasKey(us => new { us.UserId, us.SubcategoryId });

            modelBuilder.Entity<UserSubcategory>()
                .HasOne(us => us.User)
                .WithMany(u => u.UserSubcategories)
                .HasForeignKey(us => us.UserId);

            modelBuilder.Entity<UserSubcategory>()
                .HasOne(us => us.Subcategory)
                .WithMany(sc => sc.UserSubcategories)
                .HasForeignKey(us => us.SubcategoryId);

            modelBuilder.Entity<Subcategory>()
                .HasOne(sc => sc.Category)
                .WithMany(c => c.Subcategories)
                .HasForeignKey(sc => sc.CategoryId);
        }
    }
}
