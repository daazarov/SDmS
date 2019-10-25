using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Events;
using SDmS.DeviceListener.Host.Configuration;
using SDmS.DeviceListener.Infrastructure.Data.Entities;
using SDmS.DeviceListener.Infrastructure.Extensions;
using SDmS.DeviceListener.Infrastructure.Interfaces.Data;
using System;
using System.Threading.Tasks;

namespace SDmS.DeviceListener.Infrastructure.Data.Contexts
{
    public class DeviceListenerContext : IDeviceListenerContext
    {
        private readonly ILogger _logger;
        private readonly MongoSettingsModel _settings;

        private readonly IMongoDatabase _database;
        private readonly Lazy<bool> _isConnected;

        public IMongoCollection<Led> Leds => _database.GetCollection<Led>(nameof(Led));
        public IMongoCollection<TempSensor> TempSensors => _database.GetCollection<TempSensor>(nameof(TempSensor));
        public IMongoCollection<Climate> ClimateDevices => _database.GetCollection<Climate>(nameof(Climate));
        public IMongoCollection<BsonDocument> NewDevices => _database.GetCollection<BsonDocument>("new");

        public IMongoDatabase Database => _database;
        public bool IsConnected => _isConnected.Value;

        public DeviceListenerContext(MongoSettingsModel settings, ILogger<DeviceListenerContext> logger)
        {
            this._settings = settings ?? throw new ArgumentNullException(nameof(settings));
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));

            var clientSettings = ConfigureClient();

            var client = new MongoClient(clientSettings);
            _database = client.GetDatabase(settings.DbName);

            _isConnected = new Lazy<bool>(() => _database.CanConnectAsync(logger).GetAwaiter().GetResult());
        }

        private MongoClientSettings ConfigureClient()
        {
            var mongoConnectionUrl = new MongoUrl(_settings.ConnectionString);
            var mongoClientSettings = MongoClientSettings.FromUrl(mongoConnectionUrl);

            if (_settings.EnableCommandLogging)
            {
                mongoClientSettings.ClusterConfigurator = cb =>
                {
                    cb.Subscribe<CommandStartedEvent>(e =>
                    {
                        _logger.LogInformation($"{e.CommandName} - {e.Command.ToJson()}");
                    });
                };

                _logger.LogWarning("Commands logging is enabled. Performance of Mongo DB is decreased!");
            }

            return mongoClientSettings;
        }
    }
}
