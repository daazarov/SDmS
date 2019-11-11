using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace SDmS.Resource.Api.OAuth
{
    public class AuthOptions
    {
        public static SymmetricSecurityKey GetSymmetricSecurityKey(string key)
        {
            //return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            return new SymmetricSecurityKey(Convert.FromBase64String(key));
        }
    }
}
