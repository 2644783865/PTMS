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
            int? transporterId,
            int? page,
            int? pageSize);

        Task<ObjectModel> GetByIdAsync(int id);

        Task<ObjectModel> AddAsync(ObjectModel model);

        Task<ObjectModel> UpdateAsync(ObjectModel model);

        Task DeleteByIdAsync(int id);
    }
}
