using AutoMapper;
using PTMS.BusinessLogic.Helpers;
using PTMS.BusinessLogic.Infrastructure;
using PTMS.BusinessLogic.IServices;
using PTMS.BusinessLogic.Models;
using PTMS.Common;
using PTMS.DataServices.IRepositories;
using PTMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PTMS.BusinessLogic.Services
{
    public class ObjectService : BusinessServiceAsync<Objects, ObjectModel>, IObjectService
    {
        private readonly AppUserManager _userManager;
        private readonly IObjectRepository _objectRepository;

        public ObjectService(
            AppUserManager userManager,
            IObjectRepository objectRepository,
            IMapper mapper)
            : base(mapper)
        {
            _userManager = userManager;
            _objectRepository = objectRepository;
        }

        public async Task<PageResult<ObjectModel>> FindByParams(
            ClaimsPrincipal userPrincipal,
            string plateNumber,
            string routeName,
            int? carTypeId,
            int? projectId,
            ModelFormatsEnum format,
            bool? active,
            int? page,
            int? pageSize)
        {
            var userRoutesModel = await _userManager.GetAvailableRoutesModel(userPrincipal);

            var result = await _objectRepository.FindByParamsAsync(
                plateNumber,
                routeName,
                carTypeId,
                projectId,
                format,
                active,
                userRoutesModel,
                page,
                pageSize);

            return MapToModel(result);
        }

        public async Task<ObjectModel> GetByIdAsync(decimal id)
        {
            var result = await _objectRepository.GetFullByIdAsync(id);
            return MapToModel(result);
        }

        public async Task<ObjectModel> AddAsync(ObjectModel model)
        {
            var entity = MapFromModel(model);
            var result = await _objectRepository.AddAsync(entity, true);
            return MapToModel(result);
        }

        public async Task<ObjectModel> ChangeRouteAsync(
            decimal ids, 
            int newRouteId,
            ClaimsPrincipal principal)
        {
            var userRoutesModel = await _userManager.GetAvailableRoutesModel(principal);
            var entity = await _objectRepository.GetByIdAsync(ids);

            if (userRoutesModel.ProjectId.HasValue && userRoutesModel.ProjectId != entity.ProjId)
            {
                throw new InvalidOperationException("Invalid user with invalid project id");
            }

            if (userRoutesModel.RouteIds != null && !(entity.LastRout.HasValue && userRoutesModel.RouteIds.Contains(entity.LastRout.Value)))
            {
                throw new InvalidOperationException("Invalid user with invalid route ids");
            }

            if (entity.ObjOutput)
            {
                throw new InvalidOperationException("Object should be active");
            }

            entity.LastRout = newRouteId;
            await _objectRepository.UpdateAsync(entity, true);

            return await GetByIdAsync(entity.Ids);
        }

        public async Task<ObjectModel> UpdateAsync(ObjectModel model)
        {
            var entity = MapFromModel(model);
            var result = await _objectRepository.UpdateAsync(entity, true);
            return await GetByIdAsync(result.Ids);
        }

        public async Task DeleteByIdAsync(int id)
        {
            await _objectRepository.DeleteByIdAsync(id, true);
        }
    }
}
