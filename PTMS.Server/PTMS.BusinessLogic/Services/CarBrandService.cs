﻿using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using PTMS.BusinessLogic.Infrastructure;
using PTMS.BusinessLogic.IServices;
using PTMS.BusinessLogic.Models;
using PTMS.DataServices.IRepositories;
using PTMS.Domain.Entities;

namespace PTMS.BusinessLogic.Services
{
    public class CarBrandService : BusinessServiceAsync<CarBrand>, ICarBrandService
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
            return MapToModel<CarBrandModel>(result);
        }

        public async Task<CarBrandModel> AddAsync(CarBrandModel model)
        {
            var entity = MapFromModel(model);
            var result = await _carBrandRepository.AddAsync(entity);
            return MapToModel<CarBrandModel>(result);
        }

        public async Task<CarBrandModel> UpdateAsync(CarBrandModel model)
        {
            var entity = MapFromModel(model);
            var result = await _carBrandRepository.UpdateAsync(entity);
            return MapToModel<CarBrandModel>(result);
        }

        public async Task DeleteByIdAsync(int id)
        {
            await _carBrandRepository.DeleteByIdAsync(id);
        }
    }
}
