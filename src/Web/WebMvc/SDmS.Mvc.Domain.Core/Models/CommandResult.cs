using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SDmS.Domain.Core.Models
{
    public class CommandResult<T>
    {
        public HttpResponseMessage Response { get; set; }
        public T Value { get; set; }
        public string Error { get; set; }
        public int Code { get; set; }

    }
}
