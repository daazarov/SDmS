using Microsoft.Extensions.Logging;
using SDmS.DeviceEnactor.Host.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SDmS.DeviceEnactor.Host.Mqtt
{
    public class MqttReceiverMessageFactory
    {
        private readonly ILogger _logger;
        private readonly Dictionary<string, IMqttMessageProcessor> _handlers;

        public MqttReceiverMessageFactory(ILogger<MqttReceiverMessageFactory> logger, Dictionary<string, IMqttMessageProcessor> handlers)
        {
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this._handlers = handlers ?? throw new ArgumentNullException(nameof(handlers));

            Configure();
        }

        private void Configure()
        {
            // 1. get assembly
            Assembly asm = Assembly.GetCallingAssembly();

            // 2. get the list of all types in this assembly and iterate
            foreach (Type type in asm.GetTypes())
            {
                // we want the non-abstract implementations of ICommand
                if (type.IsClass && !type.IsAbstract)
                {
                    Type iHandler = type.GetInterface("SDmS.DeviceEnactor.Host.Interfaces.IMqttMessageProcessor");
                    if (iHandler != null)
                    {
                        // create an instance
                        object inst = asm.CreateInstance(type.FullName, true, BindingFlags.CreateInstance, null, null, null, null);
                        if (inst != null)
                        {
                            IMqttMessageProcessor handler = (IMqttMessageProcessor)inst;
                            // make it case insensitive
                            IMqttMessageProcessor tmp;
                            string key = handler.MessageProcessorName;
                            if (_handlers.TryGetValue(key, out tmp))
                            {
                                _logger.LogError($"MQTT message handler with name {key} is alredy initialese in handler factory");
                                continue;
                            }
                            _handlers.Add(key, handler);
                        }
                        else
                        {
                            string errMsg = $"Unable to properly initialize MqttReceiverMessageFactory - there was a problem instantiating the class {type.FullName}";
                            _logger.LogError(errMsg);
                        }
                    }
                }
            }
        }

        public IMqttMessageProcessor GetHandler(string topic)
        {
            foreach (var handler in _handlers)
            {
                if (!Regex.IsMatch(topic, handler.Value.TopicPattern))
                {
                    continue;
                }
                return handler.Value;
            }

            _logger.LogError($"No message processor found for this topic: {topic}");

            return null;
        }
    }
}
