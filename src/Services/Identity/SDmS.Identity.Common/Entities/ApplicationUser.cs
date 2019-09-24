using Microsoft.AspNet.Identity.EntityFramework;
using System;

namespace SDmS.Identity.Common.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime JoinDate { get; set; }
    }
}
