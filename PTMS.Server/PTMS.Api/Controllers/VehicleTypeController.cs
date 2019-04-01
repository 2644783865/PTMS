using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PTMS.BusinessLogic.IServices;
using PTMS.BusinessLogic.Models;

namespace PTMS.Api.Controllers
{
    public class VehicleTypeController : ApiControllerBase
    {
        private readonly IVehicleTypeService _vehicleTypeService;

        public VehicleTypeController(IVehicleTypeService vehicleTypeService)
        {
            _vehicleTypeService = vehicleTypeService;
        }
        
        [HttpGet("/vehicleTypes")]
        public async Task<ActionResult<List<VehicleTypeModel>>> GetAll()
        {
            var result = await _vehicleTypeService.GetAllAsync();
            return result;
        }
    }
}
