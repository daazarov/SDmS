using SDmS.Identity.Api.Models;
using SDmS.Identity.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SDmS.Identity.Api.Mapping
{
    public static class AccountMapping
    {
        public static ApplicationUser ViewToDomain(this AccountRegistrationModel @this)
        {
            return new ApplicationUser
            {
                Email = @this.Email,
                UserName = @this.Username,
                JoinDate = DateTime.UtcNow,
                FirstName = @this.FirstName,
                LastName = @this.LastName
            };
        }

        public static AccountCreatedResponseModel CreaterDomainToView(this ApplicationUser @this)
        {
            return new AccountCreatedResponseModel
            {
                Id = @this.Id,
                Email = @this.Email,
                UserName = @this.UserName,
                FirstName = @this.FirstName,
                LastName = @this.LastName
            };
        }
    }
}