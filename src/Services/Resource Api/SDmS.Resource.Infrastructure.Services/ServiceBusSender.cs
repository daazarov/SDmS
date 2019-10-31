using NServiceBus;
using SDmS.Resource.Infrastructure.Interfaces;
using System.Threading;
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

        public TResponse SendCallbackMessage<TResponse, UMessage>(UMessage message) where UMessage : IMessage
        {
            return _endpointInstance.Request<TResponse>(message).GetAwaiter().GetResult();
        }

        public TResponse SendCallbackMessage<TResponse, UMessage>(UMessage message, CancellationToken token) where UMessage : IMessage
        {
            return _endpointInstance.Request<TResponse>(message, token).GetAwaiter().GetResult();
        }

        public async Task<TResponse> SendCallbackMessageAsync<TResponse, UMessage>(UMessage message) where UMessage : IMessage
        {
            return await _endpointInstance.Request<TResponse>(message).ConfigureAwait(false);
        }

        public async Task<TResponse> SendCallbackMessageAsync<TResponse, UMessage>(UMessage message, CancellationToken token) where UMessage : IMessage
        {
            return await _endpointInstance.Request<TResponse>(message, token).ConfigureAwait(false);
        }
    }
}
