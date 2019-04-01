using Microsoft.AspNetCore.Authorization;
using PTMS.Common;

namespace PTMS.Api.Attributes
{
    public class PtmsAuthorizeAdmin : AuthorizeAttribute
    {
        public PtmsAuthorizeAdmin()
        {
            Roles = RoleNames.Administrator;
        }
    }
}
