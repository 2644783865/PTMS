using System.Collections.Generic;
using System.Threading.Tasks;
using PTMS.DataServices.Infrastructure;
using PTMS.Domain.Entities;

namespace PTMS.DataServices.IRepositories
{
    public interface IVehicleTypeRepository : IDataServiceAsync<VehicleType>
    {
        Task<List<VehicleType>> GetAllAsync();
    }
}
