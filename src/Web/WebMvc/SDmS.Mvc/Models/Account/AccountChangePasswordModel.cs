using System.ComponentModel.DataAnnotations;

namespace SDmS.Mvc.Models.Account
{
    public class AccountChangePasswordModel
    {
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}