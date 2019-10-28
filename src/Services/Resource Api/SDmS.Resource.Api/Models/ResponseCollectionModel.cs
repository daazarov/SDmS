using System;
using System.Collections.Generic;

namespace SDmS.Resource.Api.Models
{
    public class ResponseCollectionModel<T> : ResponseModel
    {
        public int TotalCount { get; set; }
        public Uri Next { get; set; }
        public Uri Previous { get; set; }
        public IList<T> Collection { get; set; }
    }
}
