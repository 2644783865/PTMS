using System.Linq;
using Microsoft.AspNetCore.Authorization;
using PTMS.Common;

namespace PTMS.Api.Attributes
{
    public class PtmsAuthorize : AuthorizeAttribute
    {
        public PtmsAuthorize(params string[] roles)
        {
            if (roles.Any())
            {
                Roles = RoleNames.Administrator + "," + string.Join(",", roles);
            }
        }
    }
}
