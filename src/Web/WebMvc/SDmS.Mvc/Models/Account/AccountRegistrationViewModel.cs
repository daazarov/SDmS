using FluentValidation.Attributes;
using SDmS.Mvc.Validators;
using System.ComponentModel.DataAnnotations;

namespace SDmS.Mvc.Models.Account
{
    [Validator(typeof(AccountCreationValidator))]
    public class AccountRegistrationViewModel
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        public string ConfirmCallbackUrl { get; set; }
    }
}