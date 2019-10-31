using MQTTnet.Server;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SDmS.MqttBroker.Host.Mqtt.Handlers
{
    public class MqttApplicationMessageInterceptor : IMqttServerApplicationMessageInterceptor
    {
        public Task InterceptApplicationMessagePublishAsync(MqttApplicationMessageInterceptorContext context)
        {
            // we do not process messages from ourselves
            if (string.IsNullOrEmpty(context.ClientId))
            {
                return Task.CompletedTask;
            }

            try
            {
                string payload = (context.ApplicationMessage.Payload.Length > 0) ? Encoding.UTF8.GetString(context.ApplicationMessage.Payload) : "";

                if (!string.IsNullOrEmpty(payload))
                {
                    var messageElements = JsonConvert.DeserializeObject<Dictionary<string, string>>(payload);

                    if (!messageElements.TryGetValue("client_id", out var value))
                    {
                        string clientId = context.ClientId;
                        messageElements.Add("mqtt_client_id", clientId);

                        string json = JsonConvert.SerializeObject(messageElements);

                        context.ApplicationMessage.Payload = Encoding.UTF8.GetBytes(json);
                    }
                }
                return Task.CompletedTask;
            }
            catch
            {
                return Task.CompletedTask;
            }
        }
    }
}
