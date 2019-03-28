using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PTMS.BusinessLogic.IServices;
using PTMS.BusinessLogic.Models;
using PTMS.Common;

namespace PTMS.Api.Controllers
{
    public class VehicleController : ApiControllerBase
    {
        private readonly IVehicleService _vehicleService;

        public VehicleController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }
        
        [HttpGet("/vehicles")]
        public async Task<ActionResult<PageResult<VehicleModel>>> GetAll(
            int? route = null,
            int? vehicleType = null,
            int? transporter = null,
            int? page = null,
            int? pageSize = null)
        {
            var result = await _vehicleService.FindByParams(
                route, 
                vehicleType, 
                transporter, 
                page, 
                pageSize);

            return result;
        }
        
        [HttpGet("/vehicle/{id}")]
        public async Task<ActionResult<VehicleModel>> GetById(int id)
        {
            var result = await _vehicleService.GetByIdAsync(id);
            return result;
        }
        
        [HttpPost("/vehicle")]
        public async Task<VehicleModel> Post([FromBody]VehicleModel model)
        {
            var result = await _vehicleService.AddAsync(model);
            return result;
        }
        
        [HttpPut("/vehicle/{id}")]
        public async Task<VehicleModel> Put(int id, [FromBody]VehicleModel model)
        {
            var result = await _vehicleService.UpdateAsync(model);
            return result;
        }
        
        [HttpDelete("/vehicle/{id}")]
        public async Task Delete(int id)
        {
            await _vehicleService.DeleteByIdAsync(id);
        }
    }
}
