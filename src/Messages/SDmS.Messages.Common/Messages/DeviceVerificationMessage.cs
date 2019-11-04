using SDmS.Messages.Common.Models;
using System.Text;

namespace SDmS.Messages.Common.Messages
{
    public class DeviceVerificationMessage : DeviceMessage
    {
        public string CheckType { get; set; }

        public override string ToString()
        {
            var type = GetType();

            StringBuilder builder = new StringBuilder();
            builder.AppendLine($"Command name: {type.FullName}");
            builder.AppendLine($"Serial Number: {serial_number}");
            builder.AppendLine($"Check Type: {CheckType}");

            return builder.ToString();
        }
    }
}
