using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PTMS.Api.Attributes;
using PTMS.BusinessLogic.IServices;
using PTMS.BusinessLogic.Models;
using PTMS.Common;

namespace PTMS.Api.Controllers
{
    public class ProjectController : ApiControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [PtmsAuthorize(RoleNames.Dispatcher)]
        [HttpGet("/projects")]
        public async Task<ActionResult<List<ProjectModel>>> GetAll(
            bool? active = null)
        {
            var result = await _projectService.GetAllAsync(active);
            return result;
        }

        [PtmsAuthorize(RoleNames.Dispatcher)]
        [HttpGet("/project/{id}")]
        public async Task<ActionResult<ProjectModel>> GetById(int id)
        {
            var result = await _projectService.GetByIdAsync(id);
            return result;
        }

        [PtmsAuthorize(RoleNames.Dispatcher)]
        [HttpGet("/project/byroute/{routeId}")]
        public async Task<ActionResult<ProjectModel>> GetByRouteId(int routeId)
        {
            var result = await _projectService.GetByRouteIdAsync(routeId);
            return result;
        }

        [PtmsAuthorize(RoleNames.Dispatcher)]
        [HttpPost("/project")]
        public async Task<ProjectModel> Post([FromBody]ProjectModel model)
        {
            var result = await _projectService.AddAsync(model);
            return result;
        }

        [PtmsAuthorize(RoleNames.Dispatcher)]
        [HttpPut("/project/{id}")]
        public async Task<ProjectModel> Put(int id, [FromBody]ProjectModel model)
        {
            var result = await _projectService.UpdateAsync(model);
            return result;
        }

        [PtmsAuthorize(RoleNames.Dispatcher)]
        [HttpDelete("/project/{id}")]
        public async Task Delete(int id)
        {
            await _projectService.DeleteByIdAsync(id);
        }
    }
}
