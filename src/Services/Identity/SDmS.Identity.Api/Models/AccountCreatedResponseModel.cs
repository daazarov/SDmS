using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SDmS.Identity.Api.Models
{
    public class AccountCreatedResponseModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}