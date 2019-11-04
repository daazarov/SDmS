using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using SDmS.DeviceListener.Core.Interfaces.Services;
using SDmS.DeviceListener.Infrastructure.Interfaces.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SDmS.DeviceListener.Core.Services
{
    public class MqttClientManager : IMqttClientManager
    {
        private readonly ILogger _logger;
        private readonly IDeviceListenerContext _context;

        public MqttClientManager(ILogger<NewDeviceService> logger, IDeviceListenerContext context)
        {
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this._context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task ChangeDevicesStatusAsync(bool status, string mqtt_client_id)
        {
            if (!_context.IsConnected)
            {
                _logger.LogError($"Method: {nameof(ChangeDevicesStatusAsync)} >> mongodb connection was NOT successful");
                throw new InvalidOperationException("Сonnection with MongoDB not established");
            }

            var dbCollections = await GetCollections();

            foreach (var collectionName in dbCollections)
            {
                var collection = _context.Database.GetCollection<BsonDocument>(collectionName);
                var filter = Builders<BsonDocument>.Filter.Eq("mqtt_client_id", mqtt_client_id);

                var update = Builders<BsonDocument>.Update.Set("is_online", status).CurrentDate("last_modified");
                var result = collection.UpdateMany(filter, update);
            }
        }

        private async Task<List<string>> GetCollections()
        {
            List<string> collections = new List<string>();

            foreach (BsonDocument collection in await _context.Database.ListCollectionsAsync().Result.ToListAsync<BsonDocument>())
            {
                string name = collection["name"].AsString;
                collections.Add(name);
            }

            return collections;
        }
    }
}
