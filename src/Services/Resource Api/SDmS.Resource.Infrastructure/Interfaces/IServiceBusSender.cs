using NServiceBus;
using System.Threading.Tasks;

namespace SDmS.Resource.Infrastructure.Interfaces
{
    public interface IServiceBusSender
    {
        void SendCommand(ICommand command);
        Task SendCommandAsync(ICommand command);
        void PublicEvent(IEvent @event);
        Task PublicEventAsync(IEvent @event);
        TResponse SentCallbackMessage<TResponse, UMessage>(UMessage message) where TResponse : IMessage where UMessage : ICommand;
        Task<TResponse> SentCallbackMessageAsync<TResponse, UMessage>(UMessage message) where TResponse : IMessage where UMessage : ICommand;
    }
}