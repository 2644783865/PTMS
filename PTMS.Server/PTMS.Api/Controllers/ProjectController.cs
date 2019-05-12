using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PTMS.Api.Attributes;
using PTMS.BusinessLogic.IServices;
using PTMS.BusinessLogic.Models;

namespace PTMS.Api.Controllers
{
    public class ProjectController : ApiControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }
        
        [HttpGet("/projects")]
        public async Task<ActionResult<List<ProjectModel>>> GetAll()
        {
            var result = await _projectService.GetAllAsync();
            return result;
        }

        [PtmsAuthorizeAdmin]
        [HttpGet("/project/{id}")]
        public async Task<ActionResult<ProjectModel>> GetById(int id)
        {
            var result = await _projectService.GetByIdAsync(id);
            return result;
        }

        [PtmsAuthorizeAdmin]
        [HttpPost("/project")]
        public async Task<ProjectModel> Post([FromBody]ProjectModel model)
        {
            var result = await _projectService.AddAsync(model);
            return result;
        }

        [PtmsAuthorizeAdmin]
        [HttpPut("/project/{id}")]
        public async Task<ProjectModel> Put(int id, [FromBody]ProjectModel model)
        {
            var result = await _projectService.UpdateAsync(model);
            return result;
        }

        [PtmsAuthorizeAdmin]
        [HttpDelete("/project/{id}")]
        public async Task Delete(int id)
        {
            await _projectService.DeleteByIdAsync(id);
        }
    }
}
