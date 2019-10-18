using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Client.Publishing;
using MQTTnet.Diagnostics;
using MQTTnet.Server;
using MQTTnet.Server.Status;
using SDmS.MqttBroker.Host.Configuration;
using SDmS.MqttBroker.Host.Mqtt;
using SDmS.MqttBroker.Host.Mqtt.Handlers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SDmS.MqttBroker.Host.Services
{
    public class MqttServerService
    {
        private readonly ILogger<MqttServerService> _logger;

        private readonly MqttSettingsModel _settings;
        private readonly MqttServerStorage _mqttServerStorage;
        private readonly MqttClientConnectedHandler _mqttClientConnectedHandler;
        private readonly MqttClientDisconnectedHandler _mqttClientDisconnectedHandler;
        private readonly MqttClientSubscribedTopicHandler _mqttClientSubscribedTopicHandler;
        private readonly MqttClientUnsubscribedTopicHandler _mqttClientUnsubscribedTopicHandler;
        private readonly MqttServerConnectionValidator _mqttConnectionValidator;
        private readonly MqttApplicationMessageReceivedHandler _messageHandler;
        private readonly MqttApplicationMessageInterceptor _messageInterceptor;
        private readonly IMqttServer _mqttServer;

        public MqttServerService(
            MqttSettingsModel mqttSettings,
            CustomMqttFactory mqttFactory,
            MqttClientConnectedHandler mqttClientConnectedHandler,
            MqttClientDisconnectedHandler mqttClientDisconnectedHandler,
            MqttClientSubscribedTopicHandler mqttClientSubscribedTopicHandler,
            MqttClientUnsubscribedTopicHandler mqttClientUnsubscribedTopicHandler,
            MqttServerConnectionValidator mqttConnectionValidator,
            MqttApplicationMessageReceivedHandler messageHandler,
            MqttApplicationMessageInterceptor messageInterceptor,
            MqttServerStorage mqttServerStorage,
            ILogger<MqttServerService> logger
            )
        {
            this._settings = mqttSettings ?? throw new ArgumentNullException(nameof(mqttSettings));
            this._mqttClientConnectedHandler = mqttClientConnectedHandler ?? throw new ArgumentNullException(nameof(mqttClientConnectedHandler));
            this._mqttClientDisconnectedHandler = mqttClientDisconnectedHandler ?? throw new ArgumentNullException(nameof(mqttClientDisconnectedHandler));
            this._mqttClientSubscribedTopicHandler = mqttClientSubscribedTopicHandler ?? throw new ArgumentNullException(nameof(mqttClientSubscribedTopicHandler));
            this._mqttClientUnsubscribedTopicHandler = mqttClientUnsubscribedTopicHandler ?? throw new ArgumentNullException(nameof(mqttClientUnsubscribedTopicHandler));
            this._messageHandler = messageHandler ?? throw new ArgumentNullException(nameof(messageHandler));
            this._mqttConnectionValidator = mqttConnectionValidator ?? throw new ArgumentNullException(nameof(mqttConnectionValidator));
            this._messageInterceptor = messageInterceptor ?? throw new ArgumentNullException(nameof(messageInterceptor));
            this._mqttServerStorage = mqttServerStorage ?? throw new ArgumentNullException(nameof(mqttServerStorage));
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _mqttServer = mqttFactory.CreateMqttServer();
        }

        public void Configure()
        {
            _mqttServerStorage.Configure();

            _mqttServer.ClientConnectedHandler = _mqttClientConnectedHandler;
            _mqttServer.ClientDisconnectedHandler = _mqttClientDisconnectedHandler;
            _mqttServer.ClientSubscribedTopicHandler = _mqttClientSubscribedTopicHandler;
            _mqttServer.ClientUnsubscribedTopicHandler = _mqttClientUnsubscribedTopicHandler;
            _mqttServer.ApplicationMessageReceivedHandler = _messageHandler;

            try
            {
                _mqttServer.StartAsync(CreateMqttServerOptions()).GetAwaiter().GetResult();
                _logger.LogInformation("MQTT server started.");
            }
            catch (Exception ex)
            {
                var message = new StringBuilder(ex.Message);

                var inner = ex.InnerException;
                while (inner != null)
                {
                    message.Append(" \nINNER EXCEPTION: ");
                    message.Append(inner.Message);
                    inner = inner.InnerException;
                }

                _logger.LogError(message.ToString());
            }
        }

        public Task<IList<IMqttClientStatus>> GetClientStatusAsync()
        {
            return _mqttServer.GetClientStatusAsync();
        }

        public Task<IList<IMqttSessionStatus>> GetSessionStatusAsync()
        {
            return _mqttServer.GetSessionStatusAsync();
        }

        public Task ClearRetainedApplicationMessagesAsync()
        {
            return _mqttServer.ClearRetainedApplicationMessagesAsync();
        }

        public Task<IList<MqttApplicationMessage>> GetRetainedApplicationMessagesAsync()
        {
            return _mqttServer.GetRetainedApplicationMessagesAsync();
        }

        public Task<MqttClientPublishResult> PublishAsync(MqttApplicationMessage applicationMessage)
        {
            if (applicationMessage == null) throw new ArgumentNullException(nameof(applicationMessage));

            return _mqttServer.PublishAsync(applicationMessage);
        }

        private IMqttServerOptions CreateMqttServerOptions()
        {
            var options = new MqttServerOptionsBuilder()
                .WithMaxPendingMessagesPerClient(_settings.MaxPendingMessagesPerClient)
                .WithDefaultCommunicationTimeout(TimeSpan.FromSeconds(_settings.CommunicationTimeout))
                .WithConnectionValidator(_mqttConnectionValidator)
                .WithStorage(_mqttServerStorage);

            // Configure unencrypted connections
            if (_settings.TcpEndPoint.Enabled)
            {
                options.WithDefaultEndpoint();

                if (_settings.TcpEndPoint.TryReadIPv4(out var address4))
                {
                    options.WithDefaultEndpointBoundIPAddress(address4);
                }

                if (_settings.TcpEndPoint.TryReadIPv6(out var address6))
                {
                    options.WithDefaultEndpointBoundIPV6Address(address6);
                }

                if (_settings.TcpEndPoint.Port > 0)
                {
                    options.WithDefaultEndpointPort(_settings.TcpEndPoint.Port);
                }
            }
            else
            {
                options.WithoutDefaultEndpoint();
            }

            if (_settings.ConnectionBacklog > 0)
            {
                options.WithConnectionBacklog(_settings.ConnectionBacklog);
            }

            if (_settings.EnablePersistentSessions)
            {
                options.WithPersistentSessions();
            }

            if (_settings.UseOriginalReseiverClientId)
            {
                options.WithApplicationMessageInterceptor(_messageInterceptor);
            }

            return options.Build();
        }
    }
}
