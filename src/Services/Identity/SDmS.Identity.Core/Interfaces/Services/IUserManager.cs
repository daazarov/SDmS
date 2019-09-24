using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SDmS.Identity.Core.Interfaces.Services
{
    public interface IUserManager<IUser>
    {
        Task<IUser> FindAsync(string userName, string password);
        Task<ClaimsIdentity> CreateIdentityAsync(IUser user, string authenticationType);
        Task<IdentityResult> CreateAsync(IUser user, string password);
        Task<string> GenerateEmailConfirmationTokenAsync(string userId);
        Task SendEmailAsync(string userId, string subject, string body);
        Task<IdentityResult> ConfirmEmailAsync(string userId, string token);
        Task<IUser> FindByIdAsync(string userId);
        Task<IList<string>> GetRolesAsync(string userId);
        Task<IdentityResult> RemoveFromRolesAsync(string userId, params string[] roles);
        Task<IdentityResult> AddToRolesAsync(string userId, params string[] roles);
        Task<IdentityResult> AddToRoleAsync(string userId, string role);
        Task<IUser> FindByEmailAsync(string email);
        Task<bool> IsEmailConfirmedAsync(string userId);
        Task<string> GeneratePasswordResetTokenAsync(string userId);
        Task<IdentityResult> ResetPasswordAsync(string userId, string token, string newPassword);
        Task<IUser> FindByNameAsync(string userName);
    }
}
