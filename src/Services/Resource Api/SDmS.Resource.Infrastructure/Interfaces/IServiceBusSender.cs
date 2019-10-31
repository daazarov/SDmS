using NServiceBus;
using System.Threading;
using System.Threading.Tasks;

namespace SDmS.Resource.Infrastructure.Interfaces
{
    public interface IServiceBusSender
    {
        void SendCommand(ICommand command);
        Task SendCommandAsync(ICommand command);
        void PublicEvent(IEvent @event);
        Task PublicEventAsync(IEvent @event);
        TResponse SendCallbackMessage<TResponse, UMessage>(UMessage message) where UMessage : IMessage;
		TResponse SendCallbackMessage<TResponse, UMessage>(UMessage message, CancellationToken token) where UMessage : IMessage;
        Task<TResponse> SendCallbackMessageAsync<TResponse, UMessage>(UMessage message) where UMessage : IMessage;
		Task<TResponse> SendCallbackMessageAsync<TResponse, UMessage>(UMessage message, CancellationToken token) where UMessage : IMessage;
    }
}