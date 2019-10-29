using SDmS.Infrastructure.Models.Enums;

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
                return string.Format("{0}/api/accounts/confirm_email?userId={1}&code={2}", baseUrl, userId, code);
            }

            public static string ForgotPassword(string baseUrl)
            {
                return string.Format("{0}/api/accounts/forgot_password", baseUrl);
            }

            public static string ResetPassword(string baseUrl)
            {
                return string.Format("{0}/api/accounts/reset_password", baseUrl);
            }
        }

        public static class Devices
        {
            public static string FindBySerialNumber(string baseUrl, ApiVersion version, string serialNumber, string userId)
            {
                return string.Format("{0}/api/v{1}/users/{2}/devices/{3}", baseUrl, (int)version, userId, serialNumber);
            }

            public static string AssignToUser(string baseUrl, ApiVersion version, string userId)
            {
                return string.Format("{0}/api/v{1}/users/{2}/devices", baseUrl, (int)version, userId);
            }

            public static string DeleteDevice(string baseUrl, ApiVersion version, string userId, string serialNumber)
            {
                return string.Format("{0}/api/v{1}/users/{2}/devices/{3}", baseUrl, (int)version, userId, serialNumber);
            }

            public static string GetDevices(string baseUrl, ApiVersion version, int limit, int offset, string userId, int type)
            {
                return string.Format("{0}/api/v{1}/users/{2}/devices?type={5}&limit={3}&offset={4}", baseUrl, (int)version, userId, limit, offset, type);
            }

            public static string ExecuteCommand(string baseUrl, ApiVersion version, string serialNumber, string userId)
            {
                return string.Format("{0}/api/v{1}/users/{2}/devices/{3}/commands", baseUrl, (int)version, userId, serialNumber);
            }
        }

        public static class Led
        {
            public static string ChangeLedState(string baseUrl, ApiVersion version, string serialNumber, string userId)
            {
                return string.Format("{0}/api/v{1}/users/{2}/devices/{3}/state", baseUrl, (int)version, userId, serialNumber);
            }
        }

        public static class Climate
        {

        }
    }
}
