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
            ModelFormatsEnum format = ModelFormatsEnum.Pure,
            bool? active = null)
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
                page, 
                pageSize);

            return result;
        }
        
        [HttpGet("/object/{id}")]
        public async Task<ActionResult<ObjectModel>> GetById(decimal id)
        {
            var result = await _objectService.GetByIdAsync(id);
            return result;
        }

        [HttpPost("/object/{ids}/changeRoute/{newRouteId}")]
        public async Task<ObjectModel> ChangeRoute(decimal ids, int newRouteId)
        {
            var result = await _objectService.ChangeRouteAsync(
                ids,
                newRouteId,
                User);

            return result;
        }

        [PtmsAuthorizeAdmin]
        [HttpPost("/object/{ids}/changeProvider/{providerId}")]
        public async Task<ObjectModel> ChangeProvider(decimal ids, int providerId)
        {
            var result = await _objectService.ChangeProviderAsync(
                ids,
                providerId);

            return result;
        }

        [PtmsAuthorizeAdmin]
        [HttpPost("/object/{ids}/enable/{newRouteId}")]
        public async Task<ObjectModel> EnableVehicle(decimal ids, int newRouteId)
        {
            var result = await _objectService.EnableAsync(
                ids,
                newRouteId);

            return result;
        }

        [PtmsAuthorizeAdmin]
        [HttpPost("/object/{ids}/disable")]
        public async Task<ObjectModel> DisableVehicle(decimal ids)
        {
            var result = await _objectService.DisableAsync(
                ids);

            return result;
        }

        [HttpPost("/object")]
        public async Task<ObjectModel> Post([FromBody]ObjectModel model)
        {
            var result = await _objectService.AddAsync(model);
            return result;
        }
        
        [HttpPut("/object/{id}")]
        public async Task<ObjectModel> Put(int id, [FromBody]ObjectModel model)
        {
            var result = await _objectService.UpdateAsync(model);
            return result;
        }
        
        [HttpDelete("/object/{id}")]
        public async Task Delete(int id)
        {
            await _objectService.DeleteByIdAsync(id);
        }
    }
}
