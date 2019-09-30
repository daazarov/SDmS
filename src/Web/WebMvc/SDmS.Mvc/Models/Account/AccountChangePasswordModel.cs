using FluentValidation.Attributes;
using SDmS.Mvc.Validators;
using System.ComponentModel.DataAnnotations;

namespace SDmS.Mvc.Models.Account
{
    [Validator(typeof(AccountChangePasswordValidator))]
    public class AccountChangePasswordModel
    {
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}