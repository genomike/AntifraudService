using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace AntifraudService.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();

        // Initialize the database on startup
        //using (var scope = host.Services.CreateScope())
        //{
        //    var services = scope.ServiceProvider;
        //    var initializer = services.GetRequiredService<DatabaseInitializer>();
        //    initializer.Initialize();
        //}

        host.Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}