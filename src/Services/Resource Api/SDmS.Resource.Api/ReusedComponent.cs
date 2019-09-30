using NServiceBus;
using NServiceBus.UniformSession;
using System.Threading.Tasks;

namespace SDmS.Resource.Api
{
    public class ReusedComponent
    {
        IUniformSession session;

        public ReusedComponent(IUniformSession session)
        {
            this.session = session;
        }

        public async Task SendCommand(ICommand command)
        {
            await session.Send(command).ConfigureAwait(false);
        }
    }
}
