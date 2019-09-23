using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PTMS.Api.Attributes;
using PTMS.BusinessLogic.IServices;
using PTMS.BusinessLogic.Models;
using PTMS.BusinessLogic.Models.Shared;
using PTMS.Common;

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
        public async Task<ActionResult<List<RouteModel>>> GetAll(
            int? project = null,
            bool? active = null)
        {
            var result = await _routeService.GetAllAsync(User, project, active);
            return result;
        }

        [HttpGet("/route/{id}")]
        public async Task<ActionResult<RouteModel>> GetById(int id)
        {
            var result = await _routeService.GetByIdAsync(id);
            return result;
        }

        [AllowAnonymous]
        [HttpGet("/routes/names")]
        public async Task<ActionResult<TResultModel<List<string>>>> GetAllNames()
        {
            var routeNames = await _routeService.GetAllNamesAsync();
            return new TResultModel<List<string>>(routeNames);
        }

        [PtmsAuthorize(RoleNames.Dispatcher)]
        [HttpGet("/routes/forPage")]
        public async Task<ActionResult<List<RouteFullModel>>> GetForPage()
        {
            var result = await _routeService.GetAllForPageAsync();
            return result;
        }

        [PtmsAuthorize(RoleNames.Dispatcher)]
        [HttpGet("/route/forEdit/{id}")]
        public async Task<ActionResult<RouteFullModel>> GetForEditById(int id)
        {
            var result = await _routeService.GetForEditByIdAsync(id);
            return result;
        }

        [PtmsAuthorize(RoleNames.Dispatcher)]
        [HttpPost("/route")]
        public async Task<RouteModel> Post([FromBody]RouteFullModel model)
        {
            var result = await _routeService.AddAsync(model);
            return result;
        }

        [PtmsAuthorize(RoleNames.Dispatcher)]
        [HttpPut("/route/{id}")]
        public async Task<RouteModel> Put(int id, [FromBody]RouteFullModel model)
        {
            var result = await _routeService.UpdateAsync(model);
            return result;
        }

        [PtmsAuthorize(RoleNames.Dispatcher)]
        [HttpDelete("/route/{id}")]
        public async Task Delete(int id)
        {
            await _routeService.DeleteByIdAsync(id);
        }
    }
}
