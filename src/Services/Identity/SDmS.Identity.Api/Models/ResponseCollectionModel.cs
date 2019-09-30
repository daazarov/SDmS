using System;
using System.Collections.Generic;

namespace SDmS.Identity.Api.Models
{
    public class ResponseCollectionModel<T>
    {
        public int TotalCount { get; set; }
        public Uri Next { get; set; }
        public Uri Previous { get; set; }
        public IList<T> Collection { get; set; }

        public string Error { get; set; }
        public int? ErrorCode { get; set; }
    }
}