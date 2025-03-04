using AntifraudService.Infrastructure;
using AntifraudService.Infrastructure.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AntifraudService.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Add application services
            services.AddApplicationServices();

            // Configure database
            ConfigureDatabase(services);

            // Add infrastructure services
            services.AddInfrastructureServices();

            // Add controllers
            services.AddControllers();

            // Register database initializer
            services.AddScoped<DatabaseInitializer>();
        }

        private void ConfigureDatabase(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                //var connectionString = Configuration.GetConnectionString("DefaultConnection");
                //options.UseNpgsql(connectionString);
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
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}