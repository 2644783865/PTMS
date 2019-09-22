using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PTMS.Common;
using PTMS.DataServices.Models;
using PTMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PTMS.BusinessLogic.Helpers
{
    public class AppUserManager : UserManager<AppUser>
    {
        public AppUserManager(
           IUserStore<AppUser> store,
           IOptions<IdentityOptions> optionsAccessor,
           IPasswordHasher<AppUser> passwordHasher,
           IEnumerable<IUserValidator<AppUser>> userValidators,
           IEnumerable<IPasswordValidator<AppUser>> passwordValidators,
           ILookupNormalizer keyNormalizer,
           IdentityErrorDescriber errors,
           IServiceProvider services,
           ILogger<UserManager<AppUser>> logger)
           : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
        }

        public async Task<IdentityResult> ChangePasswordAsync(AppUser user, string newPassword)
        {
            var passwordResetToken = await GeneratePasswordResetTokenAsync(user);
            var passwordResult = await ResetPasswordAsync(user, passwordResetToken, newPassword);
            return passwordResult;
        }
        
        public async Task<UserAvailableRoutes> GetAvailableRoutesModel(ClaimsPrincipal userPrincipal)
        {
            if (userPrincipal == null 
                || userPrincipal.IsInRole(RoleNames.Administrator)
                || userPrincipal.IsInRole(RoleNames.Dispatcher))
            {
                return new UserAvailableRoutes();
            }

            var user = await GetUserAsync(userPrincipal);

            var result = new UserAvailableRoutes
            {
                ProjectId = user.ProjectId,
                RouteIds = user.RouteIds
            };

            return result;
        }
    }
}
