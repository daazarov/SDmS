using FluentValidation;
using SDmS.Mvc.Models.Account;

namespace SDmS.Mvc.Validators
{
    public class AccountCreationValidator : AbstractValidator<AccountRegistrationViewModel>
    {
        public AccountCreationValidator()
        {
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.FirstName).NotEmpty().Length(3, 30);
            RuleFor(x => x.LastName).NotEmpty().Length(3, 30);
            RuleFor(x => x.Username).NotEmpty().Length(3, 30);
            RuleFor(x => x.Password).NotEmpty().Length(6, 100);
            RuleFor(x => x.ConfirmPassword).Equal(x => x.Password);
        }
    }

    public class AccountEmailModelValidator : AbstractValidator<AccountEmailModel>
    {
        public AccountEmailModelValidator()
        {
            RuleFor(x => x.Email).EmailAddress();
        }
    }

    public class AccountChangePasswordValidator : AbstractValidator<AccountChangePasswordModel>
    {
        public AccountChangePasswordValidator()
        {
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.Password).NotEmpty().Length(6, 100);
            RuleFor(x => x.ConfirmPassword).Equal(x => x.Password);
        }
    }
}