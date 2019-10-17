using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using SDmS.DeviceListener.Infrastructure.Data.Entities;
using SDmS.DeviceListener.Infrastructure.Interfaces.Data;
using SDmS.DeviceListener.Infrastructure.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SDmS.DeviceListener.Infrastructure.Data.Repositories
{
    public class LedRepository : ILedRepository
    {
        private readonly IDeviceListenerContext _context;
        private readonly ILogger _logger;

        public LedRepository(IDeviceListenerContext context, ILogger<LedRepository> logger)
        {
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this._context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task CreateAsync(Led device)
        {
            await _context.Leds.InsertOneAsync(device);
            _logger.LogInformation($"Created device document: s/n {device.serial_number}");
        }

        public async Task<DeleteResult> DeleteAsync(string id)
        {
            var result = await _context.Leds.DeleteOneAsync(x => x.id == id);

            if (result.IsAcknowledged) _logger.LogInformation($"Removed device document: _id {id}");
            else _logger.LogWarning($"Failed to remove device document: _id {id}");

            return result;
        }

        public async Task<IEnumerable<Led>> FindAsync(System.Linq.Expressions.Expression<Func<Led, bool>> filter)
        {
            return await _context.Leds.Find(filter).ToListAsync();
        }

        public async Task<Led> GetAsync(string Id)
        {
            return await _context.Leds.FindAsync(x => x.id == Id).Result.FirstOrDefaultAsync();
        }

        public async Task<Led> GetBySerialNumberAsync(string serial_number)
        {
            return await _context.Leds.FindAsync(x => x.serial_number == serial_number).Result.FirstOrDefaultAsync();
        }

        public async Task<ReplaceOneResult> ReplaceAsync(Led device)
        {
            var result = await _context.Leds.ReplaceOneAsync(x => x.id == device.id, device);

            if (result.IsAcknowledged) _logger.LogInformation($"Replaced device document: _id {device.id}");
            else _logger.LogWarning($"Failed to replace device document: _id {device.id}");

            return result;
        }

        public async Task<UpdateResult> UpdateAsync(Led device, UpdateDefinition<Led> update)
        {
            var result = await _context.Leds.UpdateOneAsync(x => x.id == device.id, update);

            if (result.IsAcknowledged) _logger.LogInformation($"Updated device document: _id {device.id}");
            else _logger.LogWarning($"Failed to update device document: _id {device.id}");

            return result;
        }
    }
}
