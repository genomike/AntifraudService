using AntifraudService.Infrastructure;
using AntifraudService.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models; // Add this using statement
using System;
using System.Linq;

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

            // Add MediatR - add this line
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Startup).Assembly));

            // If your handlers are in other assemblies (like Application), add them too:
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(Startup).Assembly);
                // Assuming you have a type in your Application project - replace with appropriate type
                cfg.RegisterServicesFromAssembly(AppDomain.CurrentDomain.GetAssemblies()
                    .FirstOrDefault(a => a.FullName.Contains("AntifraudService.Application")));
            });

            // Add Swagger services
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "AntifraudService API",
                    Version = "v1",
                    Description = "API para servicios antifraud y validador de transacciones",
                    Contact = new OpenApiContact
                    {
                        Name = "Henry",
                        Email = "genomikel@gmail.com"
                    }
                });
            });
        }

        private void ConfigureDatabase(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                var connectionString = Configuration.GetConnectionString("DefaultConnection");
                options.UseNpgsql(connectionString);
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                // Add Swagger UI for development environment
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "AntifraudService API v1");
                });
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