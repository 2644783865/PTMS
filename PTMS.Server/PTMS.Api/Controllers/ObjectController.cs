using System.Collections.Generic;
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

        [HttpGet("/object/{id}")]
        public async Task<ActionResult<ObjectModel>> GetById(int id)
        {
            var result = await _objectService.GetByIdAsync(id);
            return result;
        }

        [HttpPost("/object/{ids}/changeRoute/{newRouteId}")]
        public async Task<ObjectModel> ChangeRoute(int ids, int newRouteId)
        {
            var result = await _objectService.ChangeRouteAsync(
                ids,
                newRouteId,
                User);

            return result;
        }

        [PtmsAuthorizeAdmin]
        [HttpPost("/object/{ids}/changeProvider/{providerId}")]
        public async Task<ObjectModel> ChangeProvider(int ids, int providerId)
        {
            var result = await _objectService.ChangeProviderAsync(
                ids,
                providerId);

            return result;
        }

        [PtmsAuthorizeAdmin]
        [HttpPost("/object/{ids}/enable/{newRouteId}")]
        public async Task<ObjectModel> EnableVehicle(int ids, int newRouteId)
        {
            var result = await _objectService.EnableAsync(
                ids,
                newRouteId);

            return result;
        }

        [PtmsAuthorizeAdmin]
        [HttpPost("/object/{ids}/disable")]
        public async Task<ObjectModel> DisableVehicle(int ids)
        {
            var result = await _objectService.DisableAsync(
                ids);

            return result;
        }

        [HttpPost("/object")]
        public async Task<ObjectModel> Post([FromBody]ObjectAddEditRequest model)
        {
            var result = await _objectService.AddAsync(model);
            return result;
        }
        
        [HttpPut("/object/{id}")]
        public async Task<ObjectModel> Put(int id, [FromBody]ObjectAddEditRequest model)
        {
            var result = await _objectService.UpdateAsync(id, model);
            return result;
        }
        
        [HttpDelete("/object/{id}")]
        public async Task Delete(int id)
        {
            await _objectService.DeleteByIdAsync(id);
        }
    }
}
