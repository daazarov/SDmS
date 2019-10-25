
namespace SDmS.Infrastructure.Models
{
    public class ResponseInfrastructureModel<T>
    {
        public string Error { get; set; }
        public int? ErrorCode { get; set; }
        public T Value { get; set; }
    }
}
