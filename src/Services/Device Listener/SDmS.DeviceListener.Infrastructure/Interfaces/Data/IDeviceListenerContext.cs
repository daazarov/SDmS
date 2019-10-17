using MongoDB.Bson;
using MongoDB.Driver;
using SDmS.DeviceListener.Infrastructure.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SDmS.DeviceListener.Infrastructure.Interfaces.Data
{
    public interface IDeviceListenerContext
    {
        IMongoCollection<Led> Leds { get; }
        IMongoCollection<TempSensor> TempSensors { get; }
        IMongoCollection<Climate> ClimateDevices { get; }
        IMongoCollection<BsonDocument> NewDevices { get; }
		
		IMongoDatabase Database { get; }

        bool IsConnected { get; }
    }
}
