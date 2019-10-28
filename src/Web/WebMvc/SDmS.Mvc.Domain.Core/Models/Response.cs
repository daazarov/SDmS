
namespace SDmS.Domain.Core.Models
{
    public class Response
    {
        public string Error { get; set; }
        public int? ErrorCode { get; set; }
    }

    public class Response<T> : Response
    {
        public T Value { get; set; }
    }
}
