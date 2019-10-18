using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Client.Receiving;
using MQTTnet.Diagnostics;
using MQTTnet.Server;
using System;
using System.Text;
using System.Threading.Tasks;

namespace SDmS.MqttBroker.Host.Mqtt.Handlers
{
    public class MqttApplicationMessageReceivedHandler : IMqttApplicationMessageReceivedHandler
    {
        private readonly ILogger _logger;

        public MqttApplicationMessageReceivedHandler(ILogger<MqttServer> logger)
        {
            this._logger = logger;
        }

        public Task HandleApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs e)
        {
            Console.WriteLine("### RECEIVED APPLICATION MESSAGE ###");
            Console.WriteLine($"+ Client = {e.ClientId}");
            Console.WriteLine($"+ Topic = {e.ApplicationMessage.Topic}");
            Console.WriteLine($"+ Payload = {((e.ApplicationMessage.Payload.Length > 0) ? Encoding.UTF8.GetString(e.ApplicationMessage.Payload) : "")}");
            Console.WriteLine($"+ QoS = {e.ApplicationMessage.QualityOfServiceLevel}");
            Console.WriteLine($"+ Retain = {e.ApplicationMessage.Retain}");
            Console.WriteLine();
            return Task.CompletedTask;
        }
    }
}
