using System.Security.Claims;
using System.Threading.Tasks;
using PTMS.BusinessLogic.Models;
using PTMS.Common;

namespace PTMS.BusinessLogic.IServices
{
    public interface IVehicleService
    {
        Task<PageResult<VehicleModel>> FindByParams(
            ClaimsPrincipal user,
            string plateNumber,
            string routeName,
            int? vehicleTypeId,
            int? transporterId,
            int? page,
            int? pageSize);

        Task<VehicleModel> GetByIdAsync(int id);

        Task<VehicleModel> AddAsync(VehicleModel model);

        Task<VehicleModel> UpdateAsync(VehicleModel model);

        Task DeleteByIdAsync(int id);
    }
}
