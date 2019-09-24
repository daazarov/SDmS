using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDmS.Domain.Core.Models.Account
{
    public class JwtTokenModel
    {
        public string Token { get; set; }
        public string TokenType { get; set; }
        public DateTime Expires { get; set; }
    }
}
