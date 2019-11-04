using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using NServiceBus;
using SDmS.DeviceListener.Core.Interfaces.Services;
using SDmS.DeviceListener.Infrastructure.Interfaces.Data;
using SDmS.Messages.Common.Messages;
using SDmS.Messages.Common.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SDmS.DeviceListener.Core.Services
{
    public class NewDeviceService : INewDeviceService
    {
        private readonly ILogger _logger;
        private readonly IDeviceListenerContext _context;

        public NewDeviceService(ILogger<NewDeviceService> logger, IDeviceListenerContext context)
        {
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this._context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task RegisterDeviceAsync<T>(T device) where T : DeviceEvent
        {
            if (!_context.IsConnected)
            {
                _logger.LogError($"Serial number {device.serial_number} - registration device error, mongodb connection was NOT successful");
                throw new InvalidOperationException("Сonnection with MongoDB not established");
            }

            var dbCollections = await GetCollections();

            foreach (var collectionName in dbCollections)
            {
                var collection = _context.Database.GetCollection<BsonDocument>(collectionName);
                var filter = Builders<BsonDocument>.Filter.Eq("serial_number", device.serial_number);
                var result = await collection.FindAsync(filter);

                if (result.FirstOrDefault()== null)
                {
                    continue;
                }
                else
                {
                    _logger.LogWarning($"Device with serial number {device.serial_number} is alredy exist in DB collection");
                    return;
                }
            }

            await _context.NewDevices.InsertOneAsync(device.ToBsonDocument());
            _logger.LogInformation($"Device (serial number: {device.serial_number}) is successfully registered in the system");
        }

        public async Task<bool> AssignToUserAsync<T>(T device, string collectionName) where T : DeviceCommand
        {
            if (!_context.IsConnected)
            {
                _logger.LogError($"Serial number {device.serial_number} - device assignment error, mongodb connection was NOT successful");
                throw new InvalidOperationException("Сonnection with MongoDB not established");
            }

            var filter = Builders<BsonDocument>.Filter.Eq("serial_number", device.serial_number);
            var result = await _context.NewDevices.FindAsync(filter);
            var deviceDoc = await result.FirstOrDefaultAsync();

            if (deviceDoc == null)
            {
                _logger.LogError($"Serial number {device.serial_number} - device assignment error, device not found in MongoDb collection ({_context.NewDevices.CollectionNamespace})");
                return false;
            }

            var deleteResult = await _context.NewDevices.DeleteOneAsync(filter);

            if (deleteResult.IsAcknowledged)
            {
                var collection = _context.Database.GetCollection<BsonDocument>(collectionName);
				
				var jsonDoc = Newtonsoft.Json.JsonConvert.SerializeObject(device);
				var bsonDoc = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<BsonDocument>(jsonDoc);
				
                await collection.InsertOneAsync(bsonDoc);

                return true;
            }

            _logger.LogError($"Serial number {device.serial_number} - device assignment error, failed to remove device from collection ({_context.NewDevices.CollectionNamespace})");
            return false;
        }

        public async Task<DeviceExistenceResponseMessage> CheckDeviceExistenceInNewAsync(string serialNumber)
        {
            if (!_context.IsConnected)
            {
                _logger.LogError($"Serial number {serialNumber} - error check the existence of the device, mongodb connection was NOT successful");
                throw new InvalidOperationException("Сonnection with MongoDB not established");
            }

            var filter = Builders<BsonDocument>.Filter.Eq("serial_number", serialNumber);
            var result = await _context.NewDevices.FindAsync(filter);
            var deviceDoc = await result.FirstOrDefaultAsync();

            if (deviceDoc != null)
            {
                DeviceExistenceResponseMessage device = new DeviceExistenceResponseMessage
                {
                    is_exist = true,
                    parameters = new Dictionary<string, dynamic>()
                };

                device.device_info = new DeviceInfo();

                if (deviceDoc.Contains("mqtt_client_id"))
                {
                    device.device_info.mqtt_client_id = deviceDoc["mqtt_client_id"].AsString;
                }
                if (deviceDoc.Contains("is_online"))
                {
                    device.device_info.is_online = deviceDoc["is_online"].AsBoolean;
                }
                if (deviceDoc.Contains("type"))
                {
                    device.device_info.type_text = deviceDoc["type"].AsString;
                }

                return device;
            }
            else
            {
                return new DeviceExistenceResponseMessage { is_exist = false };
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
