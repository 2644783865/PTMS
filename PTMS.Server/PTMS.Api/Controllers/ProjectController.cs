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
        private readonly IProjectService _transporterService;

        public ProjectController(IProjectService transporterService)
        {
            _transporterService = transporterService;
        }
        
        [HttpGet("/transporters")]
        public async Task<ActionResult<List<ProjectModel>>> GetAll()
        {
            var result = await _transporterService.GetAllAsync();
            return result;
        }

        [PtmsAuthorizeAdmin]
        [HttpGet("/transporter/{id}")]
        public async Task<ActionResult<ProjectModel>> GetById(int id)
        {
            var result = await _transporterService.GetByIdAsync(id);
            return result;
        }

        [PtmsAuthorizeAdmin]
        [HttpPost("/transporter")]
        public async Task<ProjectModel> Post([FromBody]ProjectModel model)
        {
            var result = await _transporterService.AddAsync(model);
            return result;
        }

        [PtmsAuthorizeAdmin]
        [HttpPut("/transporter/{id}")]
        public async Task<ProjectModel> Put(int id, [FromBody]ProjectModel model)
        {
            var result = await _transporterService.UpdateAsync(model);
            return result;
        }

        [PtmsAuthorizeAdmin]
        [HttpDelete("/transporter/{id}")]
        public async Task Delete(int id)
        {
            await _transporterService.DeleteByIdAsync(id);
        }
    }
}
