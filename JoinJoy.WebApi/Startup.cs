using Microsoft.OpenApi.Models;
using JoinJoy.Infrastructure.Data;
using JoinJoy.Core.Interfaces;
using JoinJoy.Core.Services;
using JoinJoy.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using JoinJoy.WebApi.Middleware;
using JoinJoy.Infrastructure.Services;
using JoinJoy.Core.Models;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;

namespace JoinJoy.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Add DbContext with migrations assembly specified
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly("JoinJoy.Infrastructure")));

            var googleApiKey = Configuration["GoogleApiKey"];
            var jwtSecret = Configuration["JwtSettings:Secret"];

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAngular", builder =>
                {
                    builder.WithOrigins("http://localhost:4200")
                           .AllowAnyMethod()
                           .AllowAnyHeader()
                           .AllowCredentials();
                });
            });

            // Configure JWT authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSecret)),
                    NameClaimType = ClaimTypes.NameIdentifier
                };

                // Enable reading token from query string for SignalR requests
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/notificationHub"))
                        {
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            var geocodingApiKey = Configuration["GeocodingApi:ApiKey"];
            var huggingFaceApiKey = Configuration["HuggingFaceApiKey"];

            // Register repositories
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddSingleton(new BlobStorageService(
                Configuration["AzureBlobStorage:ConnectionString"],
                Configuration["AzureBlobStorage:ContainerName"]
            ));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ISubcategoryRepository, SubcategoryRepository>();
            services.AddScoped<ILocationRepository, LocationRepository>();
            services.AddScoped<IUserActivityRepository, UserActivityRepository>();
            services.AddScoped<IRepository<Match>, Repository<Match>>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<OpenStreetMapService>();
            services.AddHttpClient<OpenStreetMapService>();
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<IUserUnavailabilityService, UserUnavailabilityService>();
            services.AddScoped<IRepository<UserUnavailability>, Repository<UserUnavailability>>();
            services.AddScoped<IConversationRepository, ConversationRepository>();
            services.AddScoped<IRepository<Conversation>, Repository<Conversation>>();
            services.AddScoped<IRepository<Message>, Repository<Message>>();
            services.AddScoped<IMatchingService, MatchingService>();
            services.AddScoped<IUserService>(provider =>
            {
                var userRepository = provider.GetRequiredService<IRepository<User>>();
                var locationRepository = provider.GetRequiredService<ILocationRepository>();
                var userSubcategoryRepository = provider.GetRequiredService<IRepository<UserSubcategory>>();
                var blobStorageService = provider.GetRequiredService<BlobStorageService>();
                return new UserService(
                    userRepository,
                    userSubcategoryRepository,
                    googleApiKey,
                    jwtSecret,
                    locationRepository,
                    blobStorageService
                );
            });
            services.AddScoped<IFeedbackRepository, FeedbackRepository>();
            services.AddScoped<IFeedbackService, FeedbackService>();
            services.AddScoped<IActivityService>(provider =>
            {
                var userConversationRepository = provider.GetRequiredService<IRepository<UserConversation>>();
                var conversationRepository = provider.GetRequiredService<IRepository<Conversation>>();
                var feedbackRepository = provider.GetRequiredService<IRepository<Feedback>>();
                var activityRepository = provider.GetRequiredService<IRepository<Activity>>();
                var locationRepository = provider.GetRequiredService<IRepository<Location>>();
                var userRepository = provider.GetRequiredService<IUserRepository>();
                var userActivityRepository = provider.GetRequiredService<IRepository<UserActivity>>();
                var customLocationRepository = provider.GetRequiredService<ILocationRepository>();
                return new ActivityService(
                    userConversationRepository,
                    conversationRepository,
                    feedbackRepository,
                    activityRepository,
                    locationRepository,
                    userRepository,
                    userActivityRepository,
                    customLocationRepository,
                    googleApiKey,
                    geocodingApiKey
                );
            });
            services.AddScoped<ILocationService, LocationService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<ISubcategoryService, SubcategoryService>();
            services.AddHttpClient<MatchingService>();

            // Add SignalR
            services.AddSignalR().AddJsonProtocol(options =>
            {
                options.PayloadSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
                options.PayloadSerializerOptions.WriteIndented = false;
            });
            services.AddScoped<INotificationService, SignalRNotificationService>();

            services.AddAuthorization();
            services.AddLogging();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "JoinJoy API", Version = "v1" });
            });

            services.AddControllers()
                    .AddNewtonsoftJson(options =>
                    {
                        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    }).AddJsonOptions(options =>
                    {
                        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
                    });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseCors("AllowAngular");
            app.UseAuthentication();

            app.UseRouting();
            app.UseAuthorization();

            app.UseMiddleware<AuthenticationMiddleware>();
            app.UseMiddleware<ChatMiddleware>();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "JoinJoy API v1");
                c.RoutePrefix = string.Empty;
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<NotificationHub>("/notificationHub");
            });

            // Synchronously run the seeding
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var db = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                db.Database.Migrate();

                // Instead of await:
                DatabaseSeeder.SeedAsync(db).GetAwaiter().GetResult();
            }

        }
    }
}
