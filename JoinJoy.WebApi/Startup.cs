using Microsoft.OpenApi.Models;
using JoinJoy.Infrastructure.Data;
using JoinJoy.Core.Interfaces;
using JoinJoy.Core.Services;
using JoinJoy.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using JoinJoy.WebApi.Hubs;
using JoinJoy.WebApi.Middleware;
using JoinJoy.Infrastructure.Services;

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
            // Add DbContext configuration
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // Register repositories
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            // Add repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IInterestRepository, InterestRepository>();
            services.AddScoped<IHobbyRepository, HobbyRepository>();
            services.AddScoped<IActivityPreferenceRepository, ActivityPreferenceRepository>();
            services.AddScoped<IPreferredDestinationRepository, PreferredDestinationRepository>();
            services.AddScoped<IAvailabilityRepository, AvailabilityRepository>();
            // Add other repositories...

            // Add application services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IInterestService, InterestService>();
            services.AddScoped<IHobbyService, HobbyService>();
            services.AddScoped<IActivityPreferenceService, ActivityPreferenceService>();
            services.AddScoped<IPreferredDestinationService, PreferredDestinationService>();
            services.AddScoped<IAvailabilityService, AvailabilityService>();

            services.AddControllers();

            // Add SignalR
            services.AddSignalR();

            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "JoinJoy API", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

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
        }
    }
}
