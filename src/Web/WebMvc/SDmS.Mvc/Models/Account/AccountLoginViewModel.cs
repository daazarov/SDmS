using System.ComponentModel.DataAnnotations;

namespace SDmS.Mvc.Models.Account
{
    public class AccountLoginViewModel
    {
        public string Username { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}