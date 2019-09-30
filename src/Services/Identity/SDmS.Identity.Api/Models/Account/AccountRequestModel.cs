namespace SDmS.Identity.Api.Models
{
    public class AccountRequestModel
    {
        public string Email { get; set; }
        public string Username { get; set; }

        public int? limit { get; set; }
        public int? offset { get; set; }
    }
}