using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;
using NServiceBus.ObjectBuilder.MSDependencyInjection;
using SDmS.DeviceEnactor.Host.Configuration;
using SDmS.DeviceEnactor.Host.Extensions;
using SDmS.DeviceEnactor.Host.Interfaces;
using SDmS.DeviceEnactor.Host.Mqtt;
using SDmS.DeviceEnactor.Host.Mqtt.Handlers;
using SDmS.DeviceEnactor.Host.Mqtt.Logging;
using SDmS.DeviceEnactor.Host.Services;
using System;
using System.Collections.Generic;
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
                    ReadSettings(hostContext.Configuration, services);

                    services.AddHostedService<LifetimeEventsHostedService>();

                    var busSettings = new BusSettingsModel();
                    hostContext.Configuration.Bind("NServiceBus", busSettings);
                    services.AddNServiceBus(busSettings);

                    services.AddSingleton<MqttNetLoggerWrapper>();
                    services.AddSingleton<CustomMqttFactory>();
                    services.AddSingleton<MqttClientService>();

                    services.AddSingleton<MqttApplicationMessageReceivedHandler>();
                    services.AddSingleton<MqttClientConnectedHandler>();

                    services.AddSingleton<Dictionary<string, IMqttMessageProcessor>>();
                    services.AddSingleton<MqttReceiverMessageFactory>();
                })
                .ConfigureLogging((hostContext, configLogging) =>
                {
                    configLogging.AddConsole();
                })
                .UseConsoleLifetime()
                .Build();

            await host.Services.GetService<MqttClientService>().Configure();
            await host.RunAsync();
        }

        private static void ReadSettings(IConfiguration configuration, IServiceCollection services)
        {
            var mqttSettings = new MqttSettingsModel();
            configuration.Bind("MQTT", mqttSettings);
            services.AddSingleton(mqttSettings);
        }
    }
}
