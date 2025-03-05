using AntifraudService.Application.Common.Interfaces;
using AntifraudService.Infrastructure;
using AntifraudService.Infrastructure.Messaging.Kafka;
using AntifraudService.Infrastructure.Persistence;
using AntifraudService.Infrastructure.Persistence.Repositories;
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
            services.AddApplicationServices();
            ConfigureDatabase(services);
            services.AddInfrastructureServices();
            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.AllowTrailingCommas = true;
                });

            services.AddScoped<DatabaseInitializer>();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Startup).Assembly));
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(Startup).Assembly);
                cfg.RegisterServicesFromAssembly(AppDomain.CurrentDomain.GetAssemblies()
                    .FirstOrDefault(a => a.FullName.Contains("AntifraudService.Application")));
            });

            services.AddScoped<Application.Features.Antifraud.Services.TransactionValidationService>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
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

            services.AddSingleton(sp => 
            {
                var kafkaSettings = new KafkaSettings
                {
                    BootstrapServers = Configuration["Kafka:BootstrapServers"],
                    Topic = Configuration["Kafka:Topic"]
                };
                return kafkaSettings;
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