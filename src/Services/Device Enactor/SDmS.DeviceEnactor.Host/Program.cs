using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;
using Reservation;
using SDmS.DeviceEnactor.Host.Services;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SDmS.DeviceEnactor.Host
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.Title = "DeviceEnactor.Host";

            var host = new HostBuilder()
                .ConfigureHostConfiguration(configHost =>
                {
                    configHost.SetBasePath(Directory.GetCurrentDirectory());
                })
                .ConfigureAppConfiguration((hostContext, configApp) =>
                {
                    configApp.AddJsonFile("appsettings.json", optional: true);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddScoped<ReusedComponent>();

                    services.AddHostedService<LifetimeEventsHostedService>();
                    services.AddHostedService<EventBusService>();
                })
                .ConfigureLogging((hostContext, configLogging) =>
                {
                    configLogging.AddConsole();
                })
                .UseConsoleLifetime()
                .Build();

            await host.RunAsync();
        }
    }
}
