using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SDmS.Identity.Api.Models
{
    public class AccountEmailModel
    {
        public string Email { get; set; }
        public string CallbackUrl { get; set; }
    }
}