using SDmS.Mvc.Models.Enums;

namespace SDmS.Mvc.Models
{
    public class GenericMessageViewModel
    {
        public string Message { get; set; }
        public MessageTypes Type { get; set; }

        public GenericMessageViewModel()
        {
            Type = MessageTypes.info;
        }
    }
}