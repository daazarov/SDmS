using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using NServiceBus.Logging;
using NServiceBus;
using System.Text;
using System.Runtime.ExceptionServices;
using SDmS.DeviceListener.Host.NServiceBus;

namespace SDmS.DeviceListener.Host.Services
{
    public class NServiceBusService : IHostedService
    {
        private readonly ILog _logger;
        private readonly SessionAndConfigurationHolder holder;

        private IEndpointInstance _instance;
        private EndpointConfiguration _endpointConfiguration;

        public NServiceBusService(SessionAndConfigurationHolder holder)
        {
            this._logger = LogManager.GetLogger<NServiceBusService>();
            this.holder = holder;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                _instance = await Endpoint.Start(holder.EndpointConfiguration).ConfigureAwait(false);
                _logger.Info("NServiceBus service stared.");
            }
            catch (Exception e)
            {
                holder.StartupException = ExceptionDispatchInfo.Capture(e);

                var message = new StringBuilder(e.Message);

                var inner = e.InnerException;
                while (inner != null)
                {
                    message.Append(" \nINNER EXCEPTION: ");
                    message.Append(inner.Message);
                    inner = inner.InnerException;
                }

                _logger.Error(message.ToString());
                return;
            }

            holder.MessageSession = _instance;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            try
            {
                if (_instance != null)
                {
                    await _instance.Stop().ConfigureAwait(false);
                }
            }
            finally
            {
                holder.MessageSession = null;
                holder.StartupException = null;
            }
        }
    }
}
