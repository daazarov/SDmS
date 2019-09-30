namespace SDmS.Identity.Api.Models
{
    public class ResponseModel<T>
    {
        public string Error { get; set; }
        public int? ErrorCode { get; set; }
        public T Value { get; set; }
    }
}