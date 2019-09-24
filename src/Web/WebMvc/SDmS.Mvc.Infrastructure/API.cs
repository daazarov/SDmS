namespace SDmS.Mvc.Infrastructure
{
    public static class API
    {
        public static class Account
        {
            public static string Registration(string baseUrl)
            {
                return string.Format("{0}/api/accounts", baseUrl);
            }

            public static string ConfirmEmail(string baseUrl, string userId, string code)
            {
                return string.Format("{0}api/accounts/confirm_email?userId={1}&code={2}", baseUrl, userId, code);
            }

            public static string ForgotPassword(string baseUrl)
            {
                return string.Format("{0}api/accounts/forgot_password", baseUrl);
            }

            public static string ResetPassword(string baseUrl)
            {
                return string.Format("{0}api/accounts/reset_password", baseUrl);
            }
        }
    }
}
