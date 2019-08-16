using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using PTMS.BusinessLogic.Infrastructure;
using PTMS.BusinessLogic.IServices;
using PTMS.BusinessLogic.Models;
using PTMS.DataServices.IRepositories;
using PTMS.Domain.Entities;

namespace PTMS.BusinessLogic.Services
{
    public class BlockTypeService : BusinessServiceAsync<BlockType>, IBlockTypeService
    {
        private readonly IBlockTypeRepository _blockTypeRepository;

        public BlockTypeService(
            IBlockTypeRepository blockTypeRepository,
            IMapper mapper)
            : base(mapper)
        {
            _blockTypeRepository = blockTypeRepository;
        }

        public async Task<List<BlockTypeModel>> GetAllAsync()
        {
            var result = await _blockTypeRepository.GetAllAsync();
            return MapToModel<BlockTypeModel>(result);
        }
    }
}
