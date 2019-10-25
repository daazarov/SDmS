using System;

namespace SDmS.Domain.Common.Identity
{
    public class ApplicationUser
    {
        public string id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime JoinDate { get; set; }
    }
}
