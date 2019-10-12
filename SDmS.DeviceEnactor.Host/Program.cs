using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SDmS.DeviceEnactor.Host.Configuration;
using SDmS.DeviceEnactor.Host.Extensions;
using SDmS.DeviceEnactor.Host.Services;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SDmS.DeviceEnactor.Host
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Console.Title = "DeviceEnactor.Host";

            var host = new HostBuilder()
                .ConfigureHostConfiguration(configHost =>
                {
                    configHost.SetBasePath(Directory.GetCurrentDirectory());
                    configHost.AddCommandLine(args);
                })
                .ConfigureAppConfiguration((hostContext, configApp) =>
                {
                    configApp.AddJsonFile("appsettings.json", optional: true);
                    configApp.AddCommandLine(args);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<LifetimeEventsHostedService>();

                    var busSettings = new BusSettingsModel();
                    hostContext.Configuration.Bind("NServiceBus", busSettings);
                    services.AddNServiceBus(busSettings);
                })
                .ConfigureLogging((hostContext, configLogging) =>
                {
                    configLogging.AddConsole();
                })
                .UseConsoleLifetime()
                .Build();

            await host.RunAsync();
        }

        private static void ReadSettings(IConfiguration configuration, IServiceCollection services)
        {
        }
    }
}
