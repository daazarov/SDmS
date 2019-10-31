using MQTTnet;
using SDmS.Messages.Common.Models;
using SDmS.Messages.Common.Models.Enums;

namespace SDmS.DeviceEnactor.Host.Interfaces
{
    public interface IMqttMessageProcessor
    {
        string MessageProcessorName { get; }
        string TopicPattern { get; }
        MessageType Type { get; }

        DeviceEvent ParseDeviceEvent(MqttApplicationMessageReceivedEventArgs eventArgs);
        DeviceMessage ParseDeviceMessage(MqttApplicationMessageReceivedEventArgs eventArgs);
        MqttClientEvent ParseClientEvent(MqttApplicationMessageReceivedEventArgs eventArgs);
    }
}
