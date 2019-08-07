﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PTMS.Api.Attributes;
using PTMS.BusinessLogic.IServices;
using PTMS.BusinessLogic.Models;
using PTMS.Common;

namespace PTMS.Api.Controllers
{
    public class ObjectController : ApiControllerBase
    {
        private readonly IObjectService _objectService;

        public ObjectController(IObjectService objectService)
        {
            _objectService = objectService;
        }

        [PtmsAuthorize(RoleNames.Dispatcher, RoleNames.Transporter, RoleNames.Mechanic)]
        [HttpGet("/objects")]
        public async Task<ActionResult<PageResult<ObjectModel>>> GetAll(
            string plateNumber = null,
            string routeName = null,
            int? carType = null,
            int? project = null,
            int? page = null,
            int? pageSize = null,
            int? carBrand = null,
            int? provider = null,
            int? yearRelease = null,
            string blockNumber = null,
            int? blockType = null,
            ModelFormatsEnum format = ModelFormatsEnum.Pure,
            bool? active = null,
            string sortBy = "lastTime",
            OrderByEnum orderBy = OrderByEnum.Desc)
        {
            var result = await _objectService.FindByParams(
                User,
                plateNumber,
                routeName,
                carType, 
                project,
                format,
                active,
                carBrand,
                provider,
                yearRelease,
                blockNumber,
                blockType,
                sortBy,
                orderBy,
                page, 
                pageSize);

            return result;
        }

        [PtmsAuthorize(RoleNames.Dispatcher)]
        [HttpGet("/objects/reporting")]
        public async Task<ActionResult<List<ObjectModel>>> GetAllForReporting(
            int minutes = 10)
        {
            var result = await _objectService.FindForReportingAsync(minutes);
            return result;
        }

        [PtmsAuthorize(RoleNames.Dispatcher, RoleNames.Transporter, RoleNames.Mechanic)]
        [HttpGet("/object/{id}")]
        public async Task<ActionResult<ObjectModel>> GetById(int id)
        {
            var result = await _objectService.GetByIdAsync(id);
            return result;
        }

        [PtmsAuthorize(RoleNames.Dispatcher, RoleNames.Transporter, RoleNames.Mechanic)]
        [HttpPost("/object/{ids}/changeRoute/{newRouteId}")]
        public async Task<ObjectModel> ChangeRoute(int ids, int newRouteId)
        {
            var result = await _objectService.ChangeRouteAsync(
                ids,
                newRouteId,
                User);

            return result;
        }

        [PtmsAuthorize(RoleNames.Dispatcher)]
        [HttpPost("/object/{ids}/enable/{newRouteId}")]
        public async Task<ObjectModel> EnableVehicle(int ids, int newRouteId)
        {
            var result = await _objectService.EnableAsync(
                ids,
                newRouteId);

            return result;
        }

        [PtmsAuthorize(RoleNames.Dispatcher)]
        [HttpPost("/object/{ids}/disable")]
        public async Task<ObjectModel> DisableVehicle(int ids)
        {
            var result = await _objectService.DisableAsync(
                ids);

            return result;
        }

        [PtmsAuthorize(RoleNames.Dispatcher)]
        [HttpPost("/object")]
        public async Task<ObjectModel> Post([FromBody]ObjectAddEditRequest model)
        {
            var result = await _objectService.AddAsync(model);
            return result;
        }

        [PtmsAuthorize(RoleNames.Dispatcher)]
        [HttpPut("/object/{id}")]
        public async Task<ObjectModel> Put(int id, [FromBody]ObjectAddEditRequest model)
        {
            var result = await _objectService.UpdateAsync(id, model);
            return result;
        }

        [PtmsAuthorize(RoleNames.Dispatcher)]
        [HttpDelete("/object/{id}")]
        public async Task Delete(int id)
        {
            await _objectService.DeleteByIdAsync(id);
        }
    }
}
