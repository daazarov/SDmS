using System;
using System.Collections.Generic;
using System.Text;

namespace SDmS.DeviceListener.Host.Configuration
{
    public class MongoSettingsModel
    {
        public string ConnectionString { get; set; }
        public string DbName { get; set; }
		public bool EnableCommandLogging{ get; set; }
    }
}
