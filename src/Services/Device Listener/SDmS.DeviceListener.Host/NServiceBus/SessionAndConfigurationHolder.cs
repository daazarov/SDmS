using NServiceBus;
using System.Runtime.ExceptionServices;

namespace SDmS.DeviceListener.Host.NServiceBus
{
    public class SessionAndConfigurationHolder
    {
        public SessionAndConfigurationHolder(EndpointConfiguration endpointConfiguration)
        {
            EndpointConfiguration = endpointConfiguration;
        }

        public EndpointConfiguration EndpointConfiguration { get; }

        public IMessageSession MessageSession { get; internal set; }

        public ExceptionDispatchInfo StartupException { get; internal set; }
    }
}
