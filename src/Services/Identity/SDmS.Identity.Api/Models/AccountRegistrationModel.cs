using System.ComponentModel.DataAnnotations;

namespace SDmS.Identity.Api.Models
{
    public class AccountRegistrationModel
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string ConfirmCallbackUrl { get; set; }
    }
}