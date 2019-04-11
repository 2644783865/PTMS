using System.Threading.Tasks;
using PTMS.Common;
using PTMS.DataServices.Infrastructure;
using PTMS.Domain.Entities;

namespace PTMS.DataServices.IRepositories
{
    public interface IVehicleRepository : IDataServiceAsync<Vehicle>
    {
        Task<PageResult<Vehicle>> FindByParamsForPageAsync(
            string plateNumber,
            string routeName,
            int? vehicleTypeId,
            int? transporterId,
            int? page,
            int? pageSize);

        Task<Vehicle> GetByIdForPageAsync(int id);
    }
}
