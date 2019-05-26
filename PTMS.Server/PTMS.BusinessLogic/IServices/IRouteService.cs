using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using PTMS.BusinessLogic.Models;

namespace PTMS.BusinessLogic.IServices
{
    public interface IRouteService
    {
        Task<List<RouteModel>> GetAllAsync(ClaimsPrincipal userPrincipal, bool? active);

        Task<RouteModel> GetByIdAsync(int id);

        Task<RouteModel> AddAsync(RouteModel model);

        Task<RouteModel> UpdateAsync(RouteModel model);

        Task DeleteByIdAsync(int id);
    }
}
