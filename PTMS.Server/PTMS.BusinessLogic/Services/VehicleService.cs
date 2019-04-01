﻿using System.Threading.Tasks;
using AutoMapper;
using PTMS.BusinessLogic.Infrastructure;
using PTMS.BusinessLogic.IServices;
using PTMS.BusinessLogic.Models;
using PTMS.Common;
using PTMS.DataServices.IRepositories;
using PTMS.Domain.Entities;

namespace PTMS.BusinessLogic.Services
{
    public class VehicleService : BusinessServiceAsync<Vehicle, VehicleModel>, IVehicleService
    {
        private readonly IVehicleRepository _vehicleRepository;

        public VehicleService(
            IVehicleRepository vehicleRepository,
            IMapper mapper)
            : base(mapper)
        {
            _vehicleRepository = vehicleRepository;
        }

        public async Task<PageResult<VehicleModel>> FindByParams(
            int? routeId,
            int? vehicleTypeId,
            int? transporterId,
            int? page,
            int? pageSize)
        {
            var result = await _vehicleRepository.FindByParamsForPageAsync(
                routeId,
                vehicleTypeId,
                transporterId,
                page,
                pageSize);

            return MapToModel(result);
        }

        public async Task<VehicleModel> GetByIdAsync(int id)
        {
            var result = await _vehicleRepository.GetByIdForPageAsync(id);
            return MapToModel(result);
        }

        public async Task<VehicleModel> AddAsync(VehicleModel model)
        {
            var entity = MapFromModel(model);
            var result = await _vehicleRepository.AddAsync(entity, true);
            return MapToModel(result);
        }

        public async Task<VehicleModel> UpdateAsync(VehicleModel model)
        {
            var entity = MapFromModel(model);
            var result = await _vehicleRepository.UpdateAsync(entity, true);
            return MapToModel(result);
        }

        public async Task DeleteByIdAsync(int id)
        {
            await _vehicleRepository.DeleteByIdAsync(id, true);
        }
    }
}