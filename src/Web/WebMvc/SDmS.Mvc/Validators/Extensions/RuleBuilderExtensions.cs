using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SDmS.Mvc.Validators.Extensions
{
    public static class RuleBuilderExtensions1
    {
        public static IRuleBuilder<T, string> Password<T>(this IRuleBuilder<T, string> ruleBuilder, int minimumLength = 6)
        {
            var options = ruleBuilder
                .NotEmpty().WithMessage(ErrorMessages.PasswordEmpty)
                .MinimumLength(minimumLength).WithMessage(ErrorMessages.PasswordLength)
                .Matches("[A-Z]").WithMessage(ErrorMessages.PasswordUppercaseLetter)
                .Matches("[a-z]").WithMessage(ErrorMessages.PasswordLowercaseLetter)
                .Matches("[0-9]").WithMessage(ErrorMessages.PasswordDigit)
                .Matches("[^a-zA-Z0-9]").WithMessage(ErrorMessages.PasswordSpecialCharacter);
            return options;
        }
    }
}