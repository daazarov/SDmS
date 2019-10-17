using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SDmS.DeviceListener.Host.Configuration;
using SDmS.DeviceListener.Host.Extensions;
using System;
using System.IO;
using System.Threading.Tasks;
using SDmS.DeviceListener.Core.Services;

namespace SDmS.DeviceListener.Host
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            Console.Title = "DeviceListener.Host";

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
                    ReadSettings(hostContext.Configuration, services);

                    services.AddHostedService<LifetimeEventsHostedService>();

                    services.RegisterModule<InfrastructureModule>();
                    services.RegisterModule<CoreModule>();

                    services.AddNServiceBus(hostContext.Configuration);

                })
                .ConfigureLogging((hostContext, configLogging) =>
                {
                    configLogging.AddConsole();
                })
                .UseConsoleLifetime()
                .Build();

            host.Services.GetRequiredService<Infrastructure.Interfaces.Data.IDeviceListenerContext>();
            await host.RunAsync();
        }

        private static void ReadSettings(IConfiguration configuration, IServiceCollection services)
        {
            var mongoSettings = new MongoSettingsModel();
            configuration.Bind("Mongo", mongoSettings);
            services.AddSingleton(mongoSettings);
        }
    }
}
