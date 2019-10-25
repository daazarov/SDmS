using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using NServiceBus;
using SDmS.DeviceListener.Core.Interfaces.Services;
using SDmS.DeviceListener.Infrastructure.Interfaces.Data;
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
            /*if (!(device is T))
            {
                var type = typeof(T);
                _logger.LogError(new InvalidCastException(nameof(device)), $"Registration device error \nInvalid cast to type {type.FullName}");
            }

            var castDevice = device as T;*/

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
                var result = await collection.FindAsync(filter).Result.FirstOrDefaultAsync();

                if (result == null)
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

        public Task AssignToUserAsync<T>(T device, string collectionName, string userId) where T : DeviceCommand
        {
            return Task.CompletedTask;
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
