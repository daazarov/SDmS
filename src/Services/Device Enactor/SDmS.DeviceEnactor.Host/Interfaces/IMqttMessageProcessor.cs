using MQTTnet;
using SDmS.Messages.Common.Models;
using SDmS.Messages.Common.Models.Enums;

namespace SDmS.DeviceEnactor.Host.Interfaces
{
    public interface IMqttMessageProcessor
    {
        string HandlerName { get; }
        string TopicPattern { get; }
        MessageType Type { get; }

        DeviceEvent ParseEvent(MqttApplicationMessageReceivedEventArgs eventArgs);
        DeviceCommand ParseCommand(MqttApplicationMessageReceivedEventArgs eventArgs);
    }
}
