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

        [PtmsAuthorize(RoleNames.Dispatcher)]
        [HttpPost("/carBrand")]
        public async Task<CarBrandModel> Post([FromBody]CarBrandModel model)
        {
            var result = await _carBrandService.AddAsync(model);
            return result;
        }

        [PtmsAuthorize(RoleNames.Dispatcher)]
        [HttpPut("/carBrand/{id}")]
        public async Task<CarBrandModel> Put(int id, [FromBody]CarBrandModel model)
        {
            var result = await _carBrandService.UpdateAsync(model);
            return result;
        }

        [PtmsAuthorize(RoleNames.Dispatcher)]
        [HttpDelete("/carBrand/{id}")]
        public async Task Delete(int id)
        {
            await _carBrandService.DeleteByIdAsync(id);
        }
    }
}
