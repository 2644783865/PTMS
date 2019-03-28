using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PTMS.BusinessLogic.IServices;
using PTMS.BusinessLogic.Models;

namespace PTMS.Api.Controllers
{
    public class RouteController : ApiControllerBase
    {
        private readonly IRouteService _routeService;

        public RouteController(IRouteService routeService)
        {
            _routeService = routeService;
        }
        
        [HttpGet("/routes")]
        public async Task<ActionResult<List<RouteModel>>> GetAll()
        {
            var result = await _routeService.GetAllAsync();
            return result;
        }
        
        [HttpGet("/route/{id}")]
        public async Task<ActionResult<RouteModel>> GetById(int id)
        {
            var result = await _routeService.GetByIdAsync(id);
            return result;
        }
        
        [HttpPost("/route")]
        public async Task<RouteModel> Post([FromBody]RouteModel model)
        {
            var result = await _routeService.AddAsync(model);
            return result;
        }
        
        [HttpPut("/route/{id}")]
        public async Task<RouteModel> Put(int id, [FromBody]RouteModel model)
        {
            var result = await _routeService.UpdateAsync(model);
            return result;
        }
        
        [HttpDelete("/route/{id}")]
        public async Task Delete(int id)
        {
            await _routeService.DeleteByIdAsync(id);
        }
    }
}
