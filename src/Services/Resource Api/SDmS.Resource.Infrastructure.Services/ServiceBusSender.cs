using NServiceBus;
using SDmS.Resource.Infrastructure.Interfaces;
using System.Threading.Tasks;

namespace SDmS.Resource.Infrastructure.Services
{
    public class ServiceBusSender : IServiceBusSender
    {
        private readonly IEndpointInstance _endpointInstance;

        public ServiceBusSender(IEndpointInstance endpointInstance)
        {
            this._endpointInstance = endpointInstance;
        }

        public void PublicEvent(IEvent @event)
        {
            _endpointInstance.Publish(@event).GetAwaiter().GetResult();
        }

        public async Task PublicEventAsync(IEvent @event)
        {
            await _endpointInstance.Publish(@event).ConfigureAwait(false);
        }

        public void SendCommand(ICommand command)
        {
            _endpointInstance.Send(command).GetAwaiter().GetResult();
        }

        public async Task SendCommandAsync(ICommand command)
        {
            await _endpointInstance.Send(command).ConfigureAwait(false);
        }

        public TResponse SentCallbackMessage<TResponse, UMessage>(UMessage message)
            where TResponse : IMessage
            where UMessage : ICommand
        {
            return _endpointInstance.Request<TResponse>(message).GetAwaiter().GetResult();
        }

        public async Task<TResponse> SentCallbackMessageAsync<TResponse, UMessage>(UMessage message)
            where TResponse : IMessage
            where UMessage : ICommand
        {
            return await _endpointInstance.Request<TResponse>(message).ConfigureAwait(false);
        }
    }
}
