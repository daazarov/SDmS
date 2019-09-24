namespace SDmS.Identity.Api.Models
{
    public class AccountResetPasswordModel
    {
        [System.ComponentModel.DataAnnotations.EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
        public string Code { get; set; }
    }
}