using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SDmS.DeviceListener.Infrastructure.Data.Entities
{
    public class Led : MongoEntity
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string client_id { get; set; }
        public string serial_number { get; set; }
        public string type { get; set; }
        public bool online { get; set; }
        public bool enabled { get; set; }
    }
}
