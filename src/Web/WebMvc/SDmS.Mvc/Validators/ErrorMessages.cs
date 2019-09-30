namespace SDmS.Mvc.Validators
{
    public static class ErrorMessages
    {
        public const string PasswordEmpty = "Password is required";
        public const string PasswordLength = "Minimum password length is 6";
        public const string PasswordUppercaseLetter = "Password must contain uppercase letter";
        public const string PasswordLowercaseLetter = "Password must contain lowercase letter";
        public const string PasswordDigit = "Password must contain digit";
        public const string PasswordSpecialCharacter = "Password must contain special character";
        public const string PasswordConfirm = "Password and confirmation password do not match.";

        public const string LoginForbiddenWords = "Forbidden key words in login";
    }
}