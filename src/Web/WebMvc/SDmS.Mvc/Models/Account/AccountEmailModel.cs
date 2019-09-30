using FluentValidation.Attributes;
using SDmS.Mvc.Validators;

namespace SDmS.Mvc.Models.Account
{
    [Validator(typeof(AccountEmailModelValidator))]
    public class AccountEmailModel
    {
        public string Email { get; set; }
        public string CallbackUrl { get; set; }
    }
}