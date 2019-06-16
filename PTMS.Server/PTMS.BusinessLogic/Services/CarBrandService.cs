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
    public class CarBrandService : BusinessServiceAsync<CarBrand, CarBrandModel>, ICarBrandService
    {
        private readonly ICarBrandRepository _carBrandRepository;

        public CarBrandService(
            ICarBrandRepository carBrandRepository,
            IMapper mapper)
            : base(mapper)
        {
            _carBrandRepository = carBrandRepository;
        }

        public async Task<List<CarBrandModel>> GetAllAsync()
        {
            var result = await _carBrandRepository.GetAllAsync();
            return MapToModel(result);
        }
    }
}
