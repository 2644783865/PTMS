using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using PTMS.BusinessLogic.Infrastructure;
using PTMS.BusinessLogic.IServices;
using PTMS.BusinessLogic.Models;
using PTMS.DataServices.IRepositories;
using PTMS.Domain.Entities;

namespace PTMS.BusinessLogic.Services
{
    public class VehicleTypeService : BusinessServiceAsync<VehicleType, VehicleTypeModel>, IVehicleTypeService
    {
        private readonly IVehicleTypeRepository _vehicleTypeRepository;

        public VehicleTypeService(
            IVehicleTypeRepository vehicleTypeRepository,
            IMapper mapper)
            : base(mapper)
        {
            _vehicleTypeRepository = vehicleTypeRepository;
        }
        
        public async Task<List<VehicleTypeModel>> GetAllAsync()
        {
            var result = await _vehicleTypeRepository.GetAllAsync();
            return MapToModel(result);
        }
    }
}
