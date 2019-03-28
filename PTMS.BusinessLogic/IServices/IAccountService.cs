using System.Security.Claims;
using System.Threading.Tasks;
using PTMS.BusinessLogic.Models.Account;

namespace PTMS.BusinessLogic.IServices
{
    public interface IAccountService
    {
        Task<string> Login(string email, string password);
        Task<AccountIdentityModel> GetIdentityAsync(ClaimsPrincipal user);
    }
}
