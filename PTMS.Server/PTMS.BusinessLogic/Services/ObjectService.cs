using AutoMapper;
using PTMS.BusinessLogic.Helpers;
using PTMS.BusinessLogic.Infrastructure;
using PTMS.BusinessLogic.IServices;
using PTMS.BusinessLogic.Models;
using PTMS.Common;
using PTMS.DataServices.IRepositories;
using PTMS.Domain.Entities;
using PTMS.Domain.Enums;
using PTMS.Templates;
using PTMS.Templates.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PTMS.BusinessLogic.Services
{
    public class ObjectService : BusinessServiceAsync<Objects>, IObjectService
    {
        private readonly AppUserManager _userManager;
        private readonly IObjectRepository _objectRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IGranitRepository _granitRepository;
        private readonly ICarBrandRepository _carBrandRepository;
        private readonly IBusDataRepository _busDataRepository;
        private readonly EventLogCreator _eventLogCreator;
        private readonly IHtmlBuilder _htmlBuilder;
        private readonly IPdfService _pdfService;

        public ObjectService(
            AppUserManager userManager,
            IObjectRepository objectRepository,
            IProjectRepository projectRepository,
            IGranitRepository granitRepository,
            ICarBrandRepository carBrandRepository,
            IBusDataRepository busDataRepository,
            EventLogCreator eventLogCreator,
            IHtmlBuilder htmlBuilder,
            IPdfService pdfService,
            IMapper mapper)
            : base(mapper)
        {
            _userManager = userManager;
            _objectRepository = objectRepository;
            _projectRepository = projectRepository;
            _granitRepository = granitRepository;
            _carBrandRepository = carBrandRepository;
            _busDataRepository = busDataRepository;
            _eventLogCreator = eventLogCreator;
            _htmlBuilder = htmlBuilder;
            _pdfService = pdfService;
        }

        public async Task<PageResult<ObjectModel>> FindByParamsAsync(
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

            return MapToModel<ObjectModel>(result);
        }

        public async Task<byte[]> GetVehiclesPdfAsync(
            ClaimsPrincipal userPrincipal,
            string plateNumber,
            string routeName,
            int? carTypeId,
            int? projectId,
            bool? active,
            int? carBrandId,
            int? providerId,
            int? yearRelease,
            string blockNumber,
            int? blockTypeId,
            string sortBy,
            OrderByEnum orderBy)
        {
            var userRoutesModel = await _userManager.GetAvailableRoutesModel(userPrincipal);

            var vehicles = await _objectRepository.FindAllForPdfAsync(
                plateNumber,
                routeName,
                carTypeId,
                projectId,
                active,
                userRoutesModel,
                carBrandId,
                providerId,
                yearRelease,
                blockNumber,
                blockTypeId,
                sortBy,
                orderBy);

            var htmlModel = new ObjectsPrintModel(vehicles, userPrincipal.GetRoleName());
            var htmlString = await _htmlBuilder.GetObjectsTable(htmlModel);

            var pdfDoc = _pdfService.ConvertHtmlToPdf(htmlString);

            return pdfDoc;
        }

        public async Task<ObjectModel> GetByIdAsync(int id)
        {
            var result = await _objectRepository.GetPureByIdAsync(id);
            return MapToModel<ObjectModel>(result);
        }
        
        public async Task<ObjectModel> ChangeRouteAsync(
            int id, 
            int newRouteId,
            ClaimsPrincipal principal)
        {
            var userRoutesModel = await _userManager.GetAvailableRoutesModel(principal);
            var entity = await _objectRepository.GetByIdAsync(id);
            var oldEntity = entity.Clone();

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

            var updatedEntity = await _objectRepository.UpdateAsync(entity);

            await _eventLogCreator.CreateLog(principal, EventEnum.ChangeObjectRoute, oldEntity, updatedEntity);

            var result = await _objectRepository.GetFullByIdAsync(updatedEntity.Id);
            return MapToModel<ObjectModel>(result);
        }
        
        public async Task<ObjectModel> EnableAsync(
            int id,
            int newRouteId,
            ClaimsPrincipal principal)
        {
            var entity = await _objectRepository.GetByIdAsync(id);
            var oldEntity = entity.Clone();

            if (!entity.ObjOutput)
            {
                throw new InvalidOperationException("Object should be disabled");
            }

            var project = await _projectRepository.GetProjectByRouteIdAsync(newRouteId);

            entity.LastRout = newRouteId;
            entity.ProjId = project.Id;
            entity.ObjOutput = false;
            
            if (await _objectRepository.AnyByObjIdProjIdAsync(entity.ObjId, entity.ProjId, entity.Id))
            {
                entity.ObjId = await _objectRepository.GetNextObjectIdAsync();
            }

            var result = await _objectRepository.UpdateAsync(entity);
            await _eventLogCreator.CreateLog(principal, EventEnum.EnableObject, oldEntity, result);
            return await GetByIdAsync(result.Id);
        }

        public async Task<ObjectModel> DisableAsync(
            int id,
            ClaimsPrincipal principal)
        {
            var entity = await _objectRepository.GetByIdAsync(id);
            var oldEntity = entity.Clone();

            if (entity.ObjOutput)
            {
                throw new InvalidOperationException("Object should be active");
            }

            entity.ObjOutput = true;
            entity.ObjOutputDate = DateTime.Now;

            var result = await _objectRepository.UpdateAsync(entity);
            await _eventLogCreator.CreateLog(principal, EventEnum.DisableObject, oldEntity, result);
            return await GetByIdAsync(result.Id);
        }

        public async Task DeleteByIdAsync(int id, ClaimsPrincipal principal)
        {
            var entity = await _objectRepository.GetByIdWithBlockAsync(id);
            await _eventLogCreator.CreateLog(principal, EventEnum.Delete, entity, null);

            await _objectRepository.DeleteByIdAsync(id);
        }

        public async Task<List<ObjectModel>> FindForReportingAsync(int minutes)
        {
            var startDate = DateTime.Now.AddMinutes(-minutes);
            var endDate = DateTime.Now.AddMinutes(1);

            var objects = await _objectRepository.FindForReportingAsync(startDate, endDate);
            var result = MapToModel<ObjectModel>(objects);
            return result;
        }

        public async Task<ObjectModel> AddAsync(ObjectAddEditRequest request, ClaimsPrincipal principal)
        {
            await Validate(request, null);

            var entity = new Objects()
            {
                ObjOutput = true,
                DateInserted = DateTime.Now,
                ObjOutputDate = DateTime.Now
            };

            await SetNewValues(entity, request);

            var result = await _objectRepository.AddAsync(entity);
            await HandleBlock(result.Id, null, request);

            await _eventLogCreator.CreateLog(principal, EventEnum.Create, null, result);

            return await GetByIdAsync(result.Id);
        }

        public async Task<ObjectModel> UpdateAsync(int id, ObjectAddEditRequest request, ClaimsPrincipal principal)
        {
            var entity = await _objectRepository.GetByIdWithBlockAsync(id);
            var block = entity.Block;
            entity.Block = null;

            var oldEntity = entity.Clone();

            await Validate(request, entity);

            await SetNewValues(entity, request);

            var result = await _objectRepository.UpdateAsync(entity);
            await HandleBlock(entity.Id, block, request);

            if (request.UpdateBusRoutes)
            {
                await _busDataRepository.UpdateBusRoutes(result);
            }

            await _eventLogCreator.CreateLog(principal, EventEnum.Update, oldEntity, result);

            return await GetByIdAsync(result.Id);
        }

        private async Task HandleBlock(int objectId, Granit block, ObjectAddEditRequest request)
        {
            if (request.BlockTypeId.HasValue)
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
                        BlockNumber = request.BlockNumber,
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
            
            if (entity.LastRout != request.RouteId)
            {
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

            if (entity.ObjId == 0)
            {
                entity.ObjId = await _objectRepository.GetNextObjectIdAsync();
            }
            else
            {
                var entityId = entity.Id > 0 ? entity.Id : (int?)null;
                var needUpdateObjId = await _objectRepository.AnyByObjIdProjIdAsync(entity.ObjId, entity.ProjId, entityId);

                if (needUpdateObjId)
                {
                    entity.ObjId = await _objectRepository.GetNextObjectIdAsync();
                }
            }
        }

        private async Task Validate(ObjectAddEditRequest request, Objects existingEntity)
        {
            if (existingEntity == null || existingEntity.Name != request.Name)
            {
                var ifAlreadyExist = await _objectRepository.AnyByPlateNumberAsync(request.Name, existingEntity?.Id);

                if (ifAlreadyExist)
                {
                    throw new InvalidOperationException("ТС c таким номером уже существует");
                }
            }

            if (existingEntity == null || existingEntity.Phone != request.Phone)
            {
                var ifAlreadyExist = await _objectRepository.AnyByPhoneAsync(request.Phone, existingEntity?.Id);

                if (ifAlreadyExist)
                {
                    throw new InvalidOperationException("ТС c таким номером телефона (IMEI) уже существует");
                }
            }
        }
    }
}
