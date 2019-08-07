using Microsoft.AspNetCore.Mvc;
using PTMS.Api.Attributes;
using PTMS.BusinessLogic.IServices;
using PTMS.BusinessLogic.Models;
using PTMS.Common;
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

        [PtmsAuthorize(RoleNames.Dispatcher)]
        [HttpGet("/blockTypes")]
        public async Task<ActionResult<List<BlockTypeModel>>> GetAll()
        {
            var result = await _blockTypeService.GetAllAsync();
            return result;
        }
    }
}
