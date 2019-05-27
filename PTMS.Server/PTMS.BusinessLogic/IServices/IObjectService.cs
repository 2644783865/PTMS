using System.Security.Claims;
using System.Threading.Tasks;
using PTMS.BusinessLogic.Models;
using PTMS.Common;

namespace PTMS.BusinessLogic.IServices
{
    public interface IObjectService
    {
        Task<PageResult<ObjectModel>> FindByParams(
            ClaimsPrincipal user,
            string plateNumber,
            string routeName,
            int? vehicleTypeId,
            int? projectId,
            ModelFormatsEnum format,
            bool? active,
            int? page,
            int? pageSize);

        Task<ObjectModel> GetByIdAsync(decimal ids);

        Task<ObjectModel> AddAsync(ObjectModel model);

        Task<ObjectModel> UpdateAsync(ObjectModel model);

        Task<ObjectModel> ChangeRouteAsync(
            decimal ids,
            int newRouteId,
            ClaimsPrincipal principal);

        Task DeleteByIdAsync(int id);
    }
}
