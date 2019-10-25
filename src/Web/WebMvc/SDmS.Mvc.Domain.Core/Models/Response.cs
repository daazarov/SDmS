
namespace SDmS.Domain.Core.Models
{
    public class Response<T>
    {
        public string Error { get; set; }
        public int? ErrorCode { get; set; }
        public T Value { get; set; }
    }
}
