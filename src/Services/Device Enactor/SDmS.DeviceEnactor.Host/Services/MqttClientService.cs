using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using MQTTnet.Client.Publishing;
using MQTTnet.Client.Subscribing;
using MQTTnet.Client.Unsubscribing;
using NServiceBus;
using SDmS.DeviceEnactor.Host.Configuration;
using SDmS.DeviceEnactor.Host.Mqtt;
using SDmS.DeviceEnactor.Host.Mqtt.Handlers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SDmS.DeviceEnactor.Host.Services
{
    public class MqttClientService
    {
        private readonly ILogger _logger;
        private readonly MqttSettingsModel _settings;
        private readonly CustomMqttFactory _mqttFactory;
        private readonly MqttClientConnectedHandler _mqttClientConnectedHandler;
        private readonly MqttApplicationMessageReceivedHandler _mqttApplicationMessageReceivedHandler;
        private readonly IMqttClient _mqttClient;
        private readonly Lazy<IMessageSession> _eventBusSession;

        public MqttClientService(
            Lazy<IMessageSession> eventBusSession,
            ILogger<MqttClient> logger,
            MqttSettingsModel settings,
            CustomMqttFactory mqttFactory,
            MqttClientConnectedHandler mqttClientConnectedHandler,
            MqttApplicationMessageReceivedHandler mqttApplicationMessageReceivedHandler)
        {
            this._eventBusSession = eventBusSession;
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this._settings = settings ?? throw new ArgumentNullException(nameof(settings));
            this._mqttFactory = mqttFactory ?? throw new ArgumentNullException(nameof(mqttFactory));
            this._mqttClientConnectedHandler = mqttClientConnectedHandler ?? throw new ArgumentNullException(nameof(mqttClientConnectedHandler));
            this._mqttApplicationMessageReceivedHandler = mqttApplicationMessageReceivedHandler ?? throw new ArgumentNullException(nameof(mqttApplicationMessageReceivedHandler));

            // Create a new MQTT client.
            _mqttClient = _mqttFactory.CreateMqttClient();
        }

        public async Task SubscribeAsync(MqttClientSubscribeOptions options, CancellationToken cancellationToken)
        {
            await _mqttClient.SubscribeAsync(options, cancellationToken);
        }

        public async Task UnsubscribeAsync(MqttClientUnsubscribeOptions options, CancellationToken cancellationToken)
        {
            await _mqttClient.UnsubscribeAsync(options, cancellationToken);
        }

        public async Task<MqttClientPublishResult> PublishAsync(MqttApplicationMessage applicationMessage, CancellationToken cancellationToken)
        {
            return await _mqttClient.PublishAsync(applicationMessage, cancellationToken);
        }

        public async Task Configure()
        {
            var options = CreateMqttClientOptions();

            _mqttClient.ConnectedHandler = _mqttClientConnectedHandler;
            _mqttClient.ApplicationMessageReceivedHandler = _mqttApplicationMessageReceivedHandler;

            _mqttClient.UseDisconnectedHandler(async e =>
            {
                _logger.LogInformation("### DISCONNECTED FROM SERVER ###");

                await Task.Delay(TimeSpan.FromSeconds(5));

                try
                {
                    await _mqttClient.ConnectAsync(options, CancellationToken.None); // Since 3.0.5 with CancellationToken
                    await SubscribeToTopics();
                }
                catch
                {
                    _logger.LogError("### RECONNECTING FAILED ###");
                }
            });

            try
            {
                await _mqttClient.ConnectAsync(options, CancellationToken.None);
                await SubscribeToTopics();
            }
            catch (Exception e)
            {
                var message = new StringBuilder(e.Message);

                var inner = e.InnerException;
                while (inner != null)
                {
                    message.Append(" \nINNER EXCEPTION: ");
                    message.Append(inner.Message);
                    inner = inner.InnerException;
                }

                _logger.LogError(message.ToString());
            }
        }

        private IMqttClientOptions CreateMqttClientOptions()
        {
            var options = new MqttClientOptionsBuilder()
                .WithClientId(_settings.ClientId)
                .WithCredentials(_settings.Username, _settings.Password);

            if (_settings.TcpEndPoint.Enabled)
            {
                if (_settings.TcpEndPoint.Port > 0)
                {
                    options.WithTcpServer(_settings.TcpEndPoint.Server, _settings.TcpEndPoint.Port);
                }
                else
                    options.WithTcpServer(_settings.TcpEndPoint.Server);
            }
            else
            {
                options.WithTcpServer("localhost");
            }

            if (_settings.CommunicationTimeout > 0)
            {
                options.WithCommunicationTimeout(TimeSpan.FromSeconds(_settings.CommunicationTimeout));
            }

            if (_settings.EnableCleanSessions)
            {
                options.WithCleanSession();
            }

            return options.Build();
        }

        private async Task SubscribeToTopics()
        {
            if (!_mqttClient.IsConnected)
            {
                _logger.LogWarning("Subscribed error! MQTT Client is not connected");
                return;
            }

            if (_settings.ListenerTopics.Count > 0)
            {
                var topics = new List<TopicFilter>();
                var subscribeOptions = new MqttClientSubscribeOptions();

                foreach (var topic in _settings.ListenerTopics)
                {
                    topics.Add(new TopicFilterBuilder().WithTopic(topic).Build());
                }
                subscribeOptions.TopicFilters = topics;
                await SubscribeAsync(subscribeOptions, CancellationToken.None);
            }
        }
    }
}
