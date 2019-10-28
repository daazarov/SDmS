
namespace SDmS.Resource.Api.Models
{
    public class ResponseModel
    {
        public string Error { get; set; }
        public int? ErrorCode { get; set; }
        public int HttpResponseCode { get; set; }
    }

    public class ResponseModel<T> : ResponseModel
    {
        public T Value { get; set; }
    }
}
