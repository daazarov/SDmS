using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SDmS.MqttBroker.Host.Configuration;
using SDmS.MqttBroker.Host.Mqtt;
using SDmS.MqttBroker.Host.Mqtt.Handlers;
using SDmS.MqttBroker.Host.Mqtt.Logging;
using SDmS.MqttBroker.Host.Services;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SDmS.MqttBroker.Host
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Console.Title = "MQTT_Server";

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

                    services.AddSingleton<MqttNetLoggerWrapper>();
                    services.AddSingleton<CustomMqttFactory>();
                    services.AddSingleton<MqttServerService>();
                    services.AddSingleton<MqttServerStorage>();

                    services.AddSingleton<MqttClientConnectedHandler>();
                    services.AddSingleton<MqttClientDisconnectedHandler>();
                    services.AddSingleton<MqttClientSubscribedTopicHandler>();
                    services.AddSingleton<MqttClientUnsubscribedTopicHandler>();
                    services.AddSingleton<MqttServerConnectionValidator>();
                    services.AddSingleton<MqttApplicationMessageReceivedHandler>();
                    services.AddSingleton<MqttApplicationMessageInterceptor>();

                    services.AddHostedService<LifetimeEventsHostedService>();
                })
                .ConfigureLogging((hostContext, configLogging) =>
                {
                    configLogging.AddConsole();
                })
                .UseConsoleLifetime()
                .Build();

            host.Services.GetService<MqttServerService>().Configure();
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
