using Microsoft.AspNetCore.Mvc;
using PTMS.BusinessLogic.IServices;
using PTMS.BusinessLogic.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PTMS.Api.Controllers
{
    public class CarTypeController : ApiControllerBase
    {
        private readonly ICarTypeService _carTypeService;

        public CarTypeController(ICarTypeService carTypeService)
        {
            _carTypeService = carTypeService;
        }

        [HttpGet("/carTypes")]
        public async Task<ActionResult<List<CarTypeModel>>> GetAll()
        {
            var result = await _carTypeService.GetAllAsync();
            return result;
        }
    }
}
