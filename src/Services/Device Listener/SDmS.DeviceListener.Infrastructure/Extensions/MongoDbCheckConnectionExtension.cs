using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SDmS.DeviceListener.Infrastructure.Extensions
{
    public static class MongoDbCheckConnectionExtension
    {
        public static async Task<bool> CanConnectAsync(this IMongoDatabase @this)
        {
            try
            {
                await @this.RunCommandAsync((Command<BsonDocument>)"{ping:1}").ConfigureAwait(false);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
