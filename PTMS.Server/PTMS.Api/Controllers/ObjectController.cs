﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PTMS.BusinessLogic.IServices;
using PTMS.BusinessLogic.Models;
using PTMS.Common;

namespace PTMS.Api.Controllers
{
    public class ObjectController : ApiControllerBase
    {
        private readonly IObjectService _vehicleService;

        public ObjectController(IObjectService vehicleService)
        {
            _vehicleService = vehicleService;
        }
        
        [HttpGet("/vehicles")]
        public async Task<ActionResult<PageResult<ObjectModel>>> GetAll(
            string plateNumber = null,
            string routeName = null,
            int? vehicleType = null,
            int? transporter = null,
            int? page = null,
            int? pageSize = null)
        {
            var result = await _vehicleService.FindByParams(
                User,
                plateNumber,
                routeName, 
                vehicleType, 
                transporter, 
                page, 
                pageSize);

            return result;
        }
        
        [HttpGet("/vehicle/{id}")]
        public async Task<ActionResult<ObjectModel>> GetById(int id)
        {
            var result = await _vehicleService.GetByIdAsync(id);
            return result;
        }
        
        [HttpPost("/vehicle")]
        public async Task<ObjectModel> Post([FromBody]ObjectModel model)
        {
            var result = await _vehicleService.AddAsync(model);
            return result;
        }
        
        [HttpPut("/vehicle/{id}")]
        public async Task<ObjectModel> Put(int id, [FromBody]ObjectModel model)
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