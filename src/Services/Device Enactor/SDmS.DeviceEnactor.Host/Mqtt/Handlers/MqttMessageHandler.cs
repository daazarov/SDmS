using MQTTnet;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SDmS.DeviceEnactor.Host.Interfaces;
using SDmS.Messages.Common.Models;
using SDmS.Messages.Common.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SDmS.DeviceEnactor.Host.Mqtt.Handlers
{
    public abstract class MqttMessageHandler : IMqttMessageHandler
    {
        public abstract string HandlerName { get; }
        public abstract string TopicPattern { get; }
        public abstract MessageType Type { get; }

        public virtual DeviceCommand ParseCommand(MqttApplicationMessageReceivedEventArgs eventArgs) => null;

        public virtual DeviceEvent ParseEvent(MqttApplicationMessageReceivedEventArgs eventArgs) => null;

        protected bool IsValidJson(string strInput)
        {
            strInput = strInput.Trim();
            if ((strInput.StartsWith("{") && strInput.EndsWith("}")) || //For object
                (strInput.StartsWith("[") && strInput.EndsWith("]"))) //For array
            {
                try
                {
                    var obj = JToken.Parse(strInput);
                    return true;
                }
                catch (JsonReaderException jex)
                {
                    //Exception in parsing json
                    Console.WriteLine(jex.Message);
                    return false;
                }
                catch (Exception ex) //some other exception
                {
                    Console.WriteLine(ex.ToString());
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
