using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using PTMS.BusinessLogic.Models;

namespace PTMS.BusinessLogic.IServices
{
    public interface IRouteService
    {
        Task<List<RouteModel>> GetAllAsync(ClaimsPrincipal userPrincipal, int? project, bool? active);

        Task<RouteModel> GetByIdAsync(int id);

        Task<List<RouteFullModel>> GetAllForPageAsync();

        Task<RouteFullModel> GetForEditByIdAsync(int id);

        Task<RouteModel> AddAsync(RouteFullModel model);

        Task<RouteModel> UpdateAsync(RouteFullModel model);

        Task DeleteByIdAsync(int id);
    }
}
