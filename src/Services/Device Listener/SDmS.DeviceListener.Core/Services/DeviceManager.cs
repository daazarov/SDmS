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
    public class DeviceManager : IDeviceManager
    {
        private readonly ILogger _logger;
        private readonly IDeviceListenerContext _context;

        public DeviceManager(ILogger<DeviceManager> logger, IDeviceListenerContext context)
        {
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this._context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<UpdateResult> ChangeOneDeviceParameterAsync<T>(string serialNumber, string collectionName, string parameterName, T value)
        {
            var collection = _context.Database.GetCollection<BsonDocument>(collectionName);
            var filter = Builders<BsonDocument>.Filter.Eq("serial_number", serialNumber);

            var devices = await collection.FindAsync(filter);
            var device = await devices.FirstOrDefaultAsync();

            /*if (device != null)
            {
                if (!(device[parameterName] is T)) throw new InvalidOperationException();
            }*/
            //else throw new InvalidOperationException("Error. Device with serial number {serialNumber} was not found");

            var update = Builders<BsonDocument>.Update.Set(parameterName, value).CurrentDate("last_modified");
            var result = await collection.UpdateOneAsync(filter, update);

            return result;
        }

        public async Task<UpdateResult> ChangeOneDeviceParameterAsync<T>(string serialNumber, string parameterName, T value)
        {
            if (!_context.IsConnected)
            {
                _logger.LogError($"Method: {nameof(ChangeOneDeviceParameterAsync)} >> mongodb connection was NOT successful");
                throw new InvalidOperationException("Сonnection with MongoDB not established");
            }

            var dbCollections = await GetCollections();

            UpdateResult result = null;

            foreach (var collectionName in dbCollections)
            {
                var collection = _context.Database.GetCollection<BsonDocument>(collectionName);
                var filter = Builders<BsonDocument>.Filter.Eq("serial_number", serialNumber);

                var update = Builders<BsonDocument>.Update.Set(parameterName, value).CurrentDate("last_modified");
                result = await collection.UpdateOneAsync(filter, update);

                if (result.ModifiedCount > 0)
                {
                    break;
                }
            }

            return result;
        }

        public async Task<DeleteResult> DeleteOneAsync(string collectionName, string serialNumber)
        {
            var collection = _context.Database.GetCollection<BsonDocument>(collectionName);
            var filter = Builders<BsonDocument>.Filter.Eq("serial_number", serialNumber);

            var result = await collection.DeleteOneAsync(filter);

            return result;
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
