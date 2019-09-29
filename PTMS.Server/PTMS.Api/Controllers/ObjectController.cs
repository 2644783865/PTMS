using Microsoft.AspNetCore.Mvc;
using PTMS.Api.Attributes;
using PTMS.BusinessLogic.IServices;
using PTMS.BusinessLogic.Models.Object;
using PTMS.Common;
using PTMS.Common.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            var result = await _objectService.FindByParamsAsync(
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
        public async Task<ObjectModel> ChangeRoute(int ids, int newRouteId, [FromBody]ObjectChangeRouteRequest request)
        {
            var result = await _objectService.ChangeRouteAsync(
                ids,
                newRouteId,
                User,
                request.UpdateBusRoutes);

            return result;
        }

        [PtmsAuthorize(RoleNames.Dispatcher)]
        [HttpPost("/object/{id}/enable/{newRouteId}")]
        public async Task<ObjectModel> EnableVehicle(int id, int newRouteId)
        {
            var result = await _objectService.EnableAsync(
                id,
                newRouteId, 
                User);

            return result;
        }

        [PtmsAuthorize(RoleNames.Dispatcher)]
        [HttpPost("/object/{id}/disable")]
        public async Task<ObjectModel> DisableVehicle(int id)
        {
            var result = await _objectService.DisableAsync(id, User);

            return result;
        }

        [PtmsAuthorize(RoleNames.Dispatcher)]
        [HttpPost("/object")]
        public async Task<ObjectModel> Post([FromBody]ObjectAddEditRequest model)
        {
            var result = await _objectService.AddAsync(model, User);
            return result;
        }

        [PtmsAuthorize(RoleNames.Dispatcher)]
        [HttpPut("/object/{id}")]
        public async Task<ObjectModel> Put(int id, [FromBody]ObjectAddEditRequest model)
        {
            var result = await _objectService.UpdateAsync(id, model, User);
            return result;
        }

        [PtmsAuthorize(RoleNames.Dispatcher)]
        [HttpDelete("/object/{id}")]
        public async Task Delete(int id)
        {
            await _objectService.DeleteByIdAsync(id, User);
        }

        [PtmsAuthorize(RoleNames.Dispatcher, RoleNames.Transporter, RoleNames.Mechanic)]
        [HttpGet("/objects/file")]
        public async Task<IActionResult> GetPdf(
            string plateNumber = null,
            string routeName = null,
            int? carType = null,
            int? project = null,
            int? carBrand = null,
            int? provider = null,
            int? yearRelease = null,
            string blockNumber = null,
            int? blockType = null,
            bool? active = null,
            string sortBy = "lastTime",
            OrderByEnum orderBy = OrderByEnum.Desc,
            FileFormatEnum fileFormat = FileFormatEnum.Pdf)
        {
            var fileModel = await _objectService.GetVehiclesFileAsync(
                User,
                plateNumber,
                routeName,
                carType,
                project,
                active,
                carBrand,
                provider,
                yearRelease,
                blockNumber,
                blockType,
                sortBy,
                orderBy,
                fileFormat);

            return CreateFileResponse(fileModel);
        }
    }
}
