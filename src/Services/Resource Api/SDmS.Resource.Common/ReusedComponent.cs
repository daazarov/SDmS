using NServiceBus;
using NServiceBus.UniformSession;
using System.Threading.Tasks;

namespace SDmS.Resource.Common
{
    public class ReusedComponent
    {
        IUniformSession _session;

        public ReusedComponent(IUniformSession session)
        {
            this._session = session;
        }

        public async Task SendCommand(ICommand command)
        {
            await _session.Send(command).ConfigureAwait(false);
        }

        public async Task PublicEvent(IEvent @event)
        {
            await _session.Publish(@event).ConfigureAwait(false);
        }
    }
}
