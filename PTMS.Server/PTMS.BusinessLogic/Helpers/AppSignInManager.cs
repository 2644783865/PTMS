using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PTMS.Domain.Entities;
using System.Threading.Tasks;

namespace PTMS.BusinessLogic.Helpers
{
    public class AppSignInManager : SignInManager<AppUser>
    {
        public AppSignInManager(
            UserManager<AppUser> userManager,
            IHttpContextAccessor contextAccessor,
            IUserClaimsPrincipalFactory<AppUser> claimsFactory,
            IOptions<IdentityOptions> optionsAccessor,
            ILogger<SignInManager<AppUser>> logger,
            IAuthenticationSchemeProvider schemes)
            : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes)
        {

        }

        public override async Task<bool> CanSignInAsync(AppUser user)
        {
            var canSignIn = user.Enabled;
            canSignIn &= await base.CanSignInAsync(user);
            return canSignIn;
        }
    }
}
