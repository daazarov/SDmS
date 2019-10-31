using SDmS.Messages.Common.Models;

namespace SDmS.Messages.Common.Messages
{
    public class DeviceVerificationMessage : DeviceMessage
    {
        public string CheckType { get; set; }
    }
}
