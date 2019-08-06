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
        private readonly IGranitRepository _granitRepository;
        private readonly ICarBrandRepository _carBrandRepository;
        private readonly IBusDataRepository _busDataRepository;

        public ObjectService(
            AppUserManager userManager,
            IObjectRepository objectRepository,
            IProjectRepository projectRepository,
            IGranitRepository granitRepository,
            ICarBrandRepository carBrandRepository,
            IBusDataRepository busDataRepository,
            IMapper mapper)
            : base(mapper)
        {
            _userManager = userManager;
            _objectRepository = objectRepository;
            _projectRepository = projectRepository;
            _granitRepository = granitRepository;
            _carBrandRepository = carBrandRepository;
            _busDataRepository = busDataRepository;
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
            string blockNumber,
            int? blockTypeId,
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
                blockNumber,
                blockTypeId,
                sortBy,
                orderBy,
                page,
                pageSize);

            return MapToModel(result);
        }

        public async Task<ObjectModel> GetByIdAsync(int id)
        {
            var result = await _objectRepository.GetPureByIdAsync(id);
            return MapToModel(result);
        }
        
        public async Task<ObjectModel> ChangeRouteAsync(
            int id, 
            int newRouteId,
            ClaimsPrincipal principal)
        {
            var userRoutesModel = await _userManager.GetAvailableRoutesModel(principal);
            var entity = await _objectRepository.GetByIdAsync(id);

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

            var result = await _objectRepository.UpdateAsync(entity);
            await OnRouteChange(result);

            return await GetByIdAsync(result.Id);
        }
        
        public async Task<ObjectModel> EnableAsync(
            int id,
            int newRouteId)
        {
            var entity = await _objectRepository.GetByIdAsync(id);

            if (!entity.ObjOutput)
            {
                throw new InvalidOperationException("Object should be disabled");
            }

            var project = await _projectRepository.GetProjectByRouteIdAsync(newRouteId);

            entity.LastRout = newRouteId;
            entity.ProjId = project.Id;
            entity.ObjOutput = false;

            var result = await _objectRepository.UpdateAsync(entity);
            return await GetByIdAsync(result.Id);
        }

        public async Task<ObjectModel> DisableAsync(
            int id)
        {
            var entity = await _objectRepository.GetByIdAsync(id);

            if (entity.ObjOutput)
            {
                throw new InvalidOperationException("Object should be active");
            }

            entity.ObjOutput = true;
            entity.ObjOutputDate = DateTime.Now;

            var result = await _objectRepository.UpdateAsync(entity);
            return await GetByIdAsync(result.Id);
        }

        public async Task DeleteByIdAsync(int id)
        {
            await _objectRepository.DeleteByIdAsync(id);
        }

        public async Task<List<ObjectModel>> FindForReportingAsync(int minutes)
        {
            var startDate = DateTime.Now.AddMinutes(-minutes);
            var endDate = DateTime.Now.AddMinutes(1);

            var objects = await _objectRepository.FindForReportingAsync(startDate, endDate);
            var result = MapToModel(objects);
            return result;
        }

        public async Task<ObjectModel> AddAsync(ObjectAddEditRequest request)
        {
            var entity = new Objects()
            {
                ObjOutput = true,
                DateInserted = DateTime.Now,
                ObjOutputDate = DateTime.Now
            };

            await SetNewValues(entity, request);

            var ifAlreadyExist = await _objectRepository.AnyByPlateNumberAsync(entity.Name, null);

            if (ifAlreadyExist)
            {
                throw new InvalidOperationException("ТС c таким номером уже существует");
            }

            var result = await _objectRepository.AddAsync(entity);
            await HandleBlock(result.Id, null, request);

            return await GetByIdAsync(result.Id);
        }

        public async Task<ObjectModel> UpdateAsync(int id, ObjectAddEditRequest request)
        {
            var entity = await _objectRepository.GetByIdWithBlockAsync(id);
            var block = entity.Block;
            entity.Block = null;

            if (entity.Name != request.Name)
            {
                var ifAlreadyExist = await _objectRepository.AnyByPlateNumberAsync(request.Name, entity.Id);

                if (ifAlreadyExist)
                {
                    throw new InvalidOperationException("ТС c таким номером уже существует");
                }
            }

            var isNewRoute = request.RouteId.HasValue && entity.LastRout != request.RouteId;

            await SetNewValues(entity, request);

            var result = await _objectRepository.UpdateAsync(entity);
            await HandleBlock(entity.Id, block, request);

            if (isNewRoute)
            {
                await OnRouteChange(result);
            }

            return await GetByIdAsync(result.Id);
        }

        private async Task HandleBlock(int objectId, Granit block, ObjectAddEditRequest request)
        {
            if (request.BlockNumber.HasValue && request.BlockTypeId.HasValue)
            {
                if (block != null)
                {
                    block.BlockNumber = request.BlockNumber;
                    block.BlockTypeId = request.BlockTypeId.Value;

                    await _granitRepository.UpdateAsync(block);
                }
                else
                {
                    var granit = new Granit
                    {
                        ObjectId = objectId,
                        BlockNumber = request.BlockNumber.Value,
                        BlockTypeId = request.BlockTypeId.Value
                    };

                    await _granitRepository.AddAsync(granit);
                }
            }
            else
            {
                if (block != null)
                {
                    await _granitRepository.DeleteByIdAsync(block.Id);
                }
            }
        }

        private async Task SetNewValues(Objects entity, ObjectAddEditRequest request)
        {
            entity.Name = request.Name;
            entity.CarBrandId = request.CarBrandId;
            entity.YearRelease = request.YearRelease;
            entity.Phone = request.Phone;
            entity.ProviderId = request.ProviderId;

            if (entity.CarBrandId.HasValue)
            {
                entity.CarTypeId = await _carBrandRepository.GetCarTypeIdByBrandId(entity.CarBrandId.Value);
            }
            else
            {
                entity.CarTypeId = null;
            }

            entity.LastRout = request.RouteId;

            if (entity.LastRout.HasValue)
            {
                var project = await _projectRepository.GetProjectByRouteIdAsync(entity.LastRout.Value);
                entity.ProjId = project.Id;
            }
            else
            {
                entity.ProjId = 0;
            }
        }

        private async Task OnRouteChange(Objects entity)
        {
            await _busDataRepository.UpdateBusRoutes(entity);
        }
    }
}
