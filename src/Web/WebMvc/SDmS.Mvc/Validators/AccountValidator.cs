using FluentValidation;
using SDmS.Mvc.Models.Account;
using SDmS.Mvc.Validators.Extensions;

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
            RuleFor(x => x.Username).Must(x => x.ToLower().Contains("admin".ToLower())).WithMessage(ErrorMessages.LoginForbiddenWords);
            RuleFor(x => x.Password).Password();
            RuleFor(x => x.ConfirmPassword).Equal(x => x.Password).WithMessage("The password and confirmation password do not match.");
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
            RuleFor(x => x.Password).Password();
            RuleFor(x => x.ConfirmPassword).Equal(x => x.Password).WithMessage("The password and confirmation password do not match.");
        }
    }

}