using System;

namespace SDmS.Common.Entities
{
    public class AppException
    {
        public Guid Id { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
    }
}
