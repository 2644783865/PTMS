using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PTMS.BusinessLogic.IServices;
using PTMS.BusinessLogic.Models;

namespace PTMS.Api.Controllers
{
    public class TransporterController : ApiControllerBase
    {
        private readonly ITransporterService _transporterService;

        public TransporterController(ITransporterService transporterService)
        {
            _transporterService = transporterService;
        }
        
        [HttpGet("/transporters")]
        public async Task<ActionResult<List<TransporterModel>>> GetAll()
        {
            var result = await _transporterService.GetAllAsync();
            return result;
        }
        
        [HttpGet("/transporter/{id}")]
        public async Task<ActionResult<TransporterModel>> GetById(int id)
        {
            var result = await _transporterService.GetByIdAsync(id);
            return result;
        }
        
        [HttpPost("/transporter")]
        public async Task<TransporterModel> Post([FromBody]TransporterModel model)
        {
            var result = await _transporterService.AddAsync(model);
            return result;
        }
        
        [HttpPut("/transporter/{id}")]
        public async Task<TransporterModel> Put(int id, [FromBody]TransporterModel model)
        {
            var result = await _transporterService.UpdateAsync(model);
            return result;
        }
        
        [HttpDelete("/transporter/{id}")]
        public async Task Delete(int id)
        {
            await _transporterService.DeleteByIdAsync(id);
        }
    }
}
