using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace SDmS.DeviceListener.Infrastructure.Data.Entities
{
    public class TempSensor : MongoEntity
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string client_id { get; set; }
        public string serial_number { get; set; }
        public string type { get; set; }
        public bool online { get; set; }
        public double current_temperature { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime last_received_data_time { get; set; }
    }
}
