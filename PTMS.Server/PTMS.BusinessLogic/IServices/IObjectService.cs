using System.Collections.Generic;
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
            int? carTypeId,
            int? projectId,
            ModelFormatsEnum format,
            bool? active,
            int? carBrandId,
            int? providerId,
            int? yearRelease,
            string sortBy,
            OrderByEnum orderBy,
            int? page,
            int? pageSize);

        Task<ObjectModel> GetByIdAsync(decimal ids);

        Task<ObjectModel> AddAsync(ObjectModel model);

        Task<ObjectModel> UpdateAsync(ObjectModel model);

        Task<ObjectModel> ChangeRouteAsync(
            decimal ids,
            int newRouteId,
            ClaimsPrincipal principal);

        Task<ObjectModel> EnableAsync(
            decimal ids,
            int newRouteId);

        Task<ObjectModel> DisableAsync(
            decimal ids);

        Task DeleteByIdAsync(int id);

        Task<ObjectModel> ChangeProviderAsync(decimal ids, int providerId);

        Task<List<ObjectModel>> FindForReportingAsync(int minutes);
    }
}
