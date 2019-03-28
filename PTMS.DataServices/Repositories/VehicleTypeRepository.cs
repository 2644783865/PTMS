using System.Collections.Generic;
using System.Threading.Tasks;
using PTMS.DataServices.Infrastructure;
using PTMS.DataServices.IRepositories;
using PTMS.Domain.Entities;
using PTMS.Persistance;

namespace PTMS.DataServices.Repositories
{
    public class VehicleTypeRepository : DataServiceAsync<VehicleType>, IVehicleTypeRepository
    {
        public VehicleTypeRepository(ApplicationDbContext context)
            : base(context)
        {

        }

        public Task<List<VehicleType>> GetAllAsync()
        {
            return base.GetAllAsync();
        }
    }
}
