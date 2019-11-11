using System;
using System.Collections.Generic;

namespace SDmS.Infrastructure.Models
{
    public class ResponseInfrastructureCollectionModel<T>
    {
        public int TotalCount { get; set; }
        public Uri Next { get; set; }
        public Uri Previous { get; set; }
        public IEnumerable<T> Collection { get; set; }

        public string Error { get; set; }
        public int? ErrorCode { get; set; }
        public int HttpResponseCode { get; set; }
    }
}
