using Microsoft.AspNetCore.Identity;
using PTMS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace PTMS.Domain.Entities
{
    public class AppUser : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }

        public string Description { get; set; }

        public int? ProjectId { get; set; }

        public List<int> RouteIds { get; set; }

        public bool Enabled { get; set; }

        public UserStatusEnum Status
        {
            get
            {
                if (!EmailConfirmed)
                {
                    return UserStatusEnum.WaitForConfirmation;
                }
                else if (!Enabled)
                {
                    return UserStatusEnum.Disabled;
                }
                else if (LockoutEnabled && LockoutEnd.HasValue && LockoutEnd > DateTime.UtcNow)
                {
                    return UserStatusEnum.Locked;
                }
                else
                {
                    return UserStatusEnum.Active;
                }
            }
        }

        public virtual Project Project { get; set; }

        public virtual List<AppUserRole> UserRoles { get; private set; }
    }
}
