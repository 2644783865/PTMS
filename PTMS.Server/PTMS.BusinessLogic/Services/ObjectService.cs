using AutoMapper;
using PTMS.BusinessLogic.Helpers;
using PTMS.BusinessLogic.Infrastructure;
using PTMS.BusinessLogic.IServices;
using PTMS.BusinessLogic.Models;
using PTMS.Common;
using PTMS.DataServices.IRepositories;
using PTMS.Domain.Entities;
using System;
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
            int? page,
            int? pageSize)
        {
            if (userPrincipal.IsInRole(RoleNames.Transporter))
            {
                projectId = await _userManager.GetProjectId(userPrincipal);
            }

            var result = await _objectRepository.FindByParamsAsync(
                plateNumber,
                routeName,
                carTypeId,
                projectId,
                format,
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
            var projectId = await _userManager.GetProjectId(principal);
            var entity = await _objectRepository.GetByIdAsync(ids);

            if (projectId != entity.ProjId)
            {
                throw new InvalidOperationException("Invalid user with invalid project id");
            }

            //if (entity.ObjOutput)
            //{
            //    throw new InvalidOperationException("Object should be active");
            //}

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
