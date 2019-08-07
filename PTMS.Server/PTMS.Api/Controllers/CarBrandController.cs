using Microsoft.AspNetCore.Mvc;
using PTMS.Api.Attributes;
using PTMS.BusinessLogic.IServices;
using PTMS.BusinessLogic.Models;
using PTMS.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PTMS.Api.Controllers
{
    public class CarBrandController : ApiControllerBase
    {
        private readonly ICarBrandService _carBrandService;

        public CarBrandController(ICarBrandService carBrandService)
        {
            _carBrandService = carBrandService;
        }
        
        [HttpGet("/carBrands")]
        public async Task<ActionResult<List<CarBrandModel>>> GetAll()
        {
            var result = await _carBrandService.GetAllAsync();
            return result;
        }
    }
}
