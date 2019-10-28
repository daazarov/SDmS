using System.Collections.Generic;

namespace SDmS.Infrastructure.Models.Devices
{
    public class DeviceInfracstructureActionModel
    {
        private Dictionary<string, dynamic> _parameters;
        private readonly string _actionName;
        private readonly string _type;

        public DeviceInfracstructureActionModel(string actionName, string deviceType)
        {
            this._parameters = new Dictionary<string, dynamic>();
            this._actionName = actionName;
            this._type = deviceType;
        }

        public string ActionName { get { return _actionName; } }
        public string Type { get { return _type; } }
        public Dictionary<string, dynamic> Parameters { get { return _parameters; } }

        public void AddParameter(string name, dynamic value)
        {
            if (!string.IsNullOrEmpty(name) && value != null)
            {
                _parameters.Add(name, value);
            }
        }
    }
}
