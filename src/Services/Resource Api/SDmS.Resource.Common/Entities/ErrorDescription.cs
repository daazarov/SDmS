
using System;

namespace SDmS.Resource.Common.Entities
{
    public class ErrorDescription
    {
        public Guid error_id { get; set; }
        public int error_code { get; set; }
        public string description { get; set; }
    }
}
