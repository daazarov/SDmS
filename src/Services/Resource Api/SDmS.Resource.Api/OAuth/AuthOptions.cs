using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace SDmS.Resource.Api.OAuth
{
    public class AuthOptions
    {
        public static SymmetricSecurityKey GetSymmetricSecurityKey(string key)
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key));
        }
    }
}
