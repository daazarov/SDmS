using System;

namespace SDmS.Resource.Common
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
