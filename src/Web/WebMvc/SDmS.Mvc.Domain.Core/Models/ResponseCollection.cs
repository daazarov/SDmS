using System;
using System.Collections.Generic;

namespace SDmS.Domain.Core.Models
{
    public class ResponseCollection<T>
    {
        public int TotalCount { get; set; }
        public Uri Next { get; set; }
        public Uri Previous { get; set; }
        public IEnumerable<T> Collection { get; set; }

        public string Error { get; set; }
        public int? ErrorCode { get; set; }
    }
}
