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
    public class CarTypeService : BusinessServiceAsync<CarType, CarTypeModel>, ICarTypeService
    {
        private readonly ICarTypeRepository _carTypeRepository;

        public CarTypeService(
            ICarTypeRepository carTypeRepository,
            IMapper mapper)
            : base(mapper)
        {
            _carTypeRepository = carTypeRepository;
        }

        public async Task<List<CarTypeModel>> GetAllAsync()
        {
            var result = await _carTypeRepository.GetAllAsync();
            return MapToModel(result);
        }
    }
}
