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
        private readonly IProjectRepository _projectRepository;

        public ObjectService(
            AppUserManager userManager,
            IObjectRepository objectRepository,
            IProjectRepository projectRepository,
            IMapper mapper)
            : base(mapper)
        {
            _userManager = userManager;
            _objectRepository = objectRepository;
            _projectRepository = projectRepository;
        }

        public async Task<PageResult<ObjectModel>> FindByParams(
            ClaimsPrincipal userPrincipal,
            string plateNumber,
            string routeName,
            int? carTypeId,
            int? projectId,
            ModelFormatsEnum format,
            bool? active,
            int? carBrandId,
            int? providerId,
            int? yearRelease,
            string sortBy,
            OrderByEnum orderBy,
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
                carBrandId,
                providerId,
                yearRelease,
                sortBy,
                orderBy,
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
            var result = await _objectRepository.AddAsync(entity);
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

            var project = await _projectRepository.GetProjectByRouteIdAsync(newRouteId);

            entity.LastRout = newRouteId;
            entity.ProjId = project.Id;

            var result = await UpdateAsync(entity);
            return result;
        }

        public async Task<ObjectModel> ChangeProviderAsync(
            decimal ids, 
            int providerId)
        {
            var entity = await _objectRepository.GetByIdAsync(ids);
            entity.ProviderId = providerId;

            var result = await UpdateAsync(entity);
            return result;
        }

        public async Task<ObjectModel> EnableAsync(
            decimal ids,
            int newRouteId)
        {
            var entity = await _objectRepository.GetByIdAsync(ids);

            if (!entity.ObjOutput)
            {
                throw new InvalidOperationException("Object should be disabled");
            }

            var project = await _projectRepository.GetProjectByRouteIdAsync(newRouteId);

            entity.LastRout = newRouteId;
            entity.ProjId = project.Id;
            entity.ObjOutput = false;

            var result = await UpdateAsync(entity);
            return result;
        }

        public async Task<ObjectModel> DisableAsync(
            decimal ids)
        {
            var entity = await _objectRepository.GetByIdAsync(ids);

            if (entity.ObjOutput)
            {
                throw new InvalidOperationException("Object should be active");
            }

            entity.ObjOutput = true;
            entity.ObjOutputDate = DateTime.Now;

            var result = await UpdateAsync(entity);
            return result;
        }

        public async Task<ObjectModel> UpdateAsync(ObjectModel model)
        {
            var entity = MapFromModel(model);
            var result = await UpdateAsync(entity);
            return result;
        }

        public async Task DeleteByIdAsync(int id)
        {
            await _objectRepository.DeleteByIdAsync(id);
        }

        public async Task<List<ObjectModel>> FindForReportingAsync(int minutes)
        {
            var startDate = DateTime.Now.AddMinutes(-minutes);
            var endDate = DateTime.Now.AddMinutes(1);

            var objects = await _objectRepository.FindForReporting(startDate, endDate);
            var result = MapToModel(objects);
            return result;
        }

        private async Task<ObjectModel> UpdateAsync(Objects entity)
        {
            var result = await _objectRepository.UpdateAsync(entity);
            return await GetByIdAsync(result.Id);
        }
    }
}
