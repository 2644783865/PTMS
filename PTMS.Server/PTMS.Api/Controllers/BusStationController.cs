using Microsoft.AspNetCore.Mvc;
using PTMS.Api.Attributes;
using PTMS.BusinessLogic.IServices;
using PTMS.BusinessLogic.Models;
using PTMS.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PTMS.Api.Controllers
{
    public class BusStationController : ApiControllerBase
    {
        private readonly IBusStationService _busStationService;

        public BusStationController(IBusStationService busStationService)
        {
            _busStationService = busStationService;
        }

        [PtmsAuthorize(RoleNames.Dispatcher)]
        [HttpGet("/busStations")]
        public async Task<ActionResult<List<BusStationModel>>> GetAll()
        {
            var result = await _busStationService.GetAllAsync();
            return result;
        }
    }
}
