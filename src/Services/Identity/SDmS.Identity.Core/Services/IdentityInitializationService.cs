using SDmS.Identity.Common.Entities;
using SDmS.Identity.Core.Data.Context;
using SDmS.Identity.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SDmS.Identity.Core.Services
{
    public class IdentityInitializationService : IIdentityInitializationService
    {
        private readonly IUserManager<ApplicationUser> _userManager;
        private readonly IRoleManager<ApplicationRole> _roleManager;

        public IdentityInitializationService(IUserManager<ApplicationUser> userManager, IRoleManager<ApplicationRole> roleManager)
        {
            this._userManager = userManager;
            this._roleManager = roleManager;
        }

        public async Task InitializeAsync()
        {
            if (_roleManager.Roles.Count() == 0)
            {
                List<ApplicationRole> roles = new List<ApplicationRole>
                {
                    new ApplicationRole("Admin"),
                    new ApplicationRole("User")
                };

                foreach (var role in roles)
                {
                    await _roleManager.CreateAsync(role);
                }
            }

            if (await _userManager.FindByNameAsync("admin") == null)
            {
                await _userManager.CreateAsync(new ApplicationUser
                {
                    Email = "daazarov@telecom.by",
                    FirstName = "Admin",
                    LastName = "Admin",
                    JoinDate = DateTime.UtcNow,
                    UserName = "admin",
                    EmailConfirmed = true
                },
                "Stl21598z!");

                var user = await _userManager.FindByNameAsync("admin");
                await _userManager.AddToRoleAsync(user.Id, "Admin");
            }
        }
    }
}
