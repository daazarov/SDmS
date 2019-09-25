using FluentValidation.Attributes;
using System.ComponentModel.DataAnnotations;

namespace SDmS.Mvc.Models.Account
{
    [Validator(typeof(AccountRegistrationViewModel))]
    public class AccountRegistrationViewModel
    {
        public string Email { get; set; }
        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        public string ConfirmCallbackUrl { get; set; }
    }
}