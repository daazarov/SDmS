using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SDmS.DeviceListener.Infrastructure.Data.Entities
{
    public class Device
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        public string device_id { get; set; }
        public string mqtt_client_id { get; set; }
        public string type_text { get; set; }
        public string serial_number { get; set; }
        public bool is_online { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? last_modified { get; set; }
        public BsonDocument parameters { get; set; }
    }
}
