﻿using Microsoft.OpenApi.Models;
using JoinJoy.Infrastructure.Data;
using JoinJoy.Core.Interfaces;
using JoinJoy.Core.Services;
using JoinJoy.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using JoinJoy.WebApi.Hubs;
using JoinJoy.WebApi.Middleware;
using JoinJoy.Infrastructure.Services;
using JoinJoy.Core.Models;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

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
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPreferredDestinationRepository, PreferredDestinationRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ISubcategoryRepository, SubcategoryRepository>();
            services.AddScoped<ILocationRepository, LocationRepository>(); // Ensure LocationRepository is registered
            services.AddScoped<IUserActivityRepository, UserActivityRepository>();
            services.AddScoped<IRepository<Match>, Repository<Match>>();
            // Register message service and repository
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<IMessageRepository, MessageRepository>();  // Add this line
            services.AddScoped<IConversationRepository, ConversationRepository>();
            services.AddScoped<IRepository<Conversation>, Repository<Conversation>>();
            services.AddScoped<IRepository<Message>, Repository<Message>>();
            services.AddScoped<IMatchingService, MatchingService>();
            services.AddScoped<IUserService>(provider =>
            {
                var userRepository = provider.GetRequiredService<IRepository<User>>();
                var userSubcategoryRepository = provider.GetRequiredService<IRepository<UserSubcategory>>();
                var userPreferredDestinationRepository = provider.GetRequiredService<IRepository<UserPreferredDestination>>();
                return new UserService(
                    userRepository,
                    userSubcategoryRepository,
                    userPreferredDestinationRepository,
                    googleApiKey
                );
            });
            services.AddScoped<IActivityService>(provider =>
            {
                var activityRepository = provider.GetRequiredService<IRepository<Activity>>();
                var locationRepository = provider.GetRequiredService<IRepository<Location>>();
                var userRepository = provider.GetRequiredService<IUserRepository>();
                var userActivityRepository = provider.GetRequiredService<IRepository<UserActivity>>();
                var customLocationRepository = provider.GetRequiredService<ILocationRepository>();
                return new ActivityService(
                    activityRepository,
                    locationRepository,
                    userRepository,
                    userActivityRepository,
                    customLocationRepository,
                    googleApiKey
                );
            });
            // Register AIChatService with HttpClient
            services.AddHttpClient<IAIChatService, AIChatService>(client =>
            {
                client.BaseAddress = new Uri("https://api-inference.huggingface.co/models/microsoft/phi-1_5");
            }).ConfigurePrimaryHttpMessageHandler(() =>
            {
                return new HttpClientHandler
                {
                    AutomaticDecompression = System.Net.DecompressionMethods.GZip
                };
            });
            services.AddHttpClient<FlaskApiService>(client =>
            {
                client.BaseAddress = new Uri("http://localhost:5000"); // Flask API base address
            });
            services.AddScoped<IPreferredDestinationService, PreferredDestinationService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ISubcategoryService, SubcategoryService>();
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
            app.UseRouting();

            app.UseAuthentication();
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
                endpoints.MapHub<ChatHub>("/chathub");
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
