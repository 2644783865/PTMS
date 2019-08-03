using Microsoft.AspNetCore.Mvc;
using PTMS.BusinessLogic.IServices;
using PTMS.BusinessLogic.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PTMS.Api.Controllers
{
    public class BlockTypeController : ApiControllerBase
    {
        private readonly IBlockTypeService _blockTypeService;

        public BlockTypeController(IBlockTypeService blockTypeService)
        {
            _blockTypeService = blockTypeService;
        }

        [HttpGet("/blockTypes")]
        public async Task<ActionResult<List<BlockTypeModel>>> GetAll()
        {
            var result = await _blockTypeService.GetAllAsync();
            return result;
        }
    }
}
