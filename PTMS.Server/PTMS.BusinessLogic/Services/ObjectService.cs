using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using PTMS.BusinessLogic.Infrastructure;
using PTMS.BusinessLogic.IServices;
using PTMS.BusinessLogic.Models;
using PTMS.Common;
using PTMS.DataServices.IRepositories;
using PTMS.Domain.Entities;

namespace PTMS.BusinessLogic.Services
{
    public class ObjectService : BusinessServiceAsync<Objects, ObjectModel>, IObjectService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IObjectRepository _objectRepository;

        public ObjectService(
            UserManager<AppUser> userManager,
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
            int? vehicleTypeId,
            int? projectId,
            int? page,
            int? pageSize)
        {
            if (userPrincipal.IsInRole(RoleNames.Transporter))
            {
                var user = await _userManager.GetUserAsync(userPrincipal);
                projectId = user.ProjectId;
            }            

            var result = await _objectRepository.FindByParamsForPageAsync(
                plateNumber,
                routeName,
                vehicleTypeId,
                projectId,
                page,
                pageSize);

            return MapToModel(result);
        }

        public async Task<ObjectModel> GetByIdAsync(int id)
        {
            var result = await _objectRepository.GetByIdForPageAsync(id);
            return MapToModel(result);
        }

        public async Task<ObjectModel> AddAsync(ObjectModel model)
        {
            var entity = MapFromModel(model);
            var result = await _objectRepository.AddAsync(entity, true);
            return MapToModel(result);
        }

        public async Task<ObjectModel> UpdateAsync(ObjectModel model)
        {
            var entity = MapFromModel(model);
            var result = await _objectRepository.UpdateAsync(entity, true);
            return MapToModel(result);
        }

        public async Task DeleteByIdAsync(int id)
        {
            await _objectRepository.DeleteByIdAsync(id, true);
        }
    }
}
