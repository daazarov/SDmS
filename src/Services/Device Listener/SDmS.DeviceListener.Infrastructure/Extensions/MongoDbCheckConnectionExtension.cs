using Microsoft.Extensions.Logging;
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
        public static async Task<bool> CanConnectAsync(this IMongoDatabase @this, ILogger logger = null)
        {
            try
            {
                if(logger != null) logger.LogInformation($"Check connection to MongoDB...");

                await @this.RunCommandAsync((Command<BsonDocument>)"{ping:1}").ConfigureAwait(false);

                if (logger != null) logger.LogInformation($"Connection to {@this.DatabaseNamespace.DatabaseName} DB was successful!");

                return true;
            }
            catch (AggregateException e)
            {
                if (e.InnerException is MongoAuthenticationException)
                {
                    if (logger != null) logger.LogError($"Connection to {@this.DatabaseNamespace.DatabaseName} DB was NOT successful! Details: {e.Message}");
                }
                if (e.InnerException is TimeoutException)
                {
                    if (logger != null) logger.LogError($"Connection to {@this.DatabaseNamespace.DatabaseName} DB was NOT successful! Details: Connection timeout");
                }

                return false;
            }
            catch (Exception e)
            {
                if (logger != null) logger.LogError(e, e.Message);

                return false;
            }
        }
    }
}
