using FluentValidation;
using System.Text.RegularExpressions;

namespace SDmS.Mvc.Validators.Extensions
{
    public static class RuleBuilderExtensions
    {
        public static IRuleBuilder<T, string> Password<T>(this IRuleBuilder<T, string> ruleBuilder, int minimumLength = 6)
        {
            var options = ruleBuilder
                .NotEmpty().WithMessage(ErrorMessages.PasswordEmpty)
                .MinimumLength(minimumLength).WithMessage(ErrorMessages.PasswordLength)
                .Must(x => Regex.IsMatch(x, "[A-Z]")).WithMessage(ErrorMessages.PasswordUppercaseLetter)
                .Must(x => Regex.IsMatch(x, "[a-z]")).WithMessage(ErrorMessages.PasswordLowercaseLetter)
                .Must(x => Regex.IsMatch(x, "[0-9]")).WithMessage(ErrorMessages.PasswordDigit)
                .Must(x => Regex.IsMatch(x, "[^a-zA-Z0-9]")).WithMessage(ErrorMessages.PasswordSpecialCharacter);
            return options;
        }
    }
}