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
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
            // CORS policy for Angular frontend
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAngular", builder =>
                {
                    builder.WithOrigins("http://localhost:4200")  // Angular URL
                           .AllowAnyMethod()
                           .AllowAnyHeader();
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
                    NameClaimType = ClaimTypes.NameIdentifier  // Sets the default claim for user identity

                };
            });
            var geocodingApiKey = Configuration["GeocodingApi:ApiKey"];
            var huggingFaceApiKey = Configuration["HuggingFaceApiKey"];
            // Add CORS policy to allow Angular frontend
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAngular", builder =>
                {
                    builder.WithOrigins("http://localhost:4200")  // Angular URL
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });
            // Register repositories
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddSingleton(new BlobStorageService(
                Configuration["AzureBlobStorage:ConnectionString"],
                Configuration["AzureBlobStorage:ContainerName"]
            ));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ISubcategoryRepository, SubcategoryRepository>();
            services.AddScoped<ILocationRepository, LocationRepository>(); // Ensure LocationRepository is registered
            services.AddScoped<IUserActivityRepository, UserActivityRepository>();
            services.AddScoped<IRepository<Match>, Repository<Match>>();
            // Register message service and repository
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<OpenStreetMapService>();
            services.AddHttpClient<OpenStreetMapService>();
            services.AddScoped<IMessageRepository, MessageRepository>();  // Add this line
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
                var feedbackRepository = provider.GetRequiredService<IRepository<Feedback>>();
                var activityRepository = provider.GetRequiredService<IRepository<Activity>>();
                var locationRepository = provider.GetRequiredService<IRepository<Location>>();
                var userRepository = provider.GetRequiredService<IUserRepository>();
                var userActivityRepository = provider.GetRequiredService<IRepository<UserActivity>>();
                var customLocationRepository = provider.GetRequiredService<ILocationRepository>();
                return new ActivityService(
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


            services.AddAuthorization();

            services.AddLogging();
            // Add SignalR
            services.AddSignalR();

            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "JoinJoy API", Version = "v1" });
            });

            services.AddControllers()
                    .AddNewtonsoftJson(options =>
                    {
                        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ApplicationDbContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseCors("AllowAngular");  // Enable the CORS policy
            app.UseAuthentication();

            app.UseRouting();



            app.UseAuthorization();

            app.UseMiddleware<AuthenticationMiddleware>();
            app.UseMiddleware<ChatMiddleware>();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "JoinJoy API v1");
                c.RoutePrefix = string.Empty;  // Set Swagger UI at the root of the application
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // Ensure the database is seeded
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var _context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                DatabaseSeeder.SeedAsync(context).Wait();
            }
        }
    }
}
