﻿namespace SDmS.Domain.Core.Models
{
    public class AccountResetPasswordModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Code { get; set; }
    }
}
