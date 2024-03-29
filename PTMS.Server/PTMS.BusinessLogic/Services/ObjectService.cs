﻿using AutoMapper;
using PTMS.BusinessLogic.Helpers;
using PTMS.BusinessLogic.Infrastructure;
using PTMS.BusinessLogic.IServices;
using PTMS.BusinessLogic.Models.Object;
using PTMS.BusinessLogic.Models.Shared;
using PTMS.Common;
using PTMS.Common.Enums;
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
        private readonly IXlsxBuilder _xlsxBuilder;
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
            IXlsxBuilder xlsxBuilder,
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
            _xlsxBuilder = xlsxBuilder;
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

        public async Task<FileModel> GetVehiclesFileAsync(
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
            OrderByEnum orderBy,
            FileFormatEnum fileFormat)
        {
            var userRoutesModel = await _userManager.GetAvailableRoutesModel(userPrincipal);

            var vehicles = await _objectRepository.FindAllForFileAsync(
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

            var printFile = new ObjectsPrintModel(vehicles, userPrincipal.GetRoleName())
            {
                PlateNumber = plateNumber,
                RouteName = routeName,
                CarTypeId = carTypeId,
                ProjectId = projectId,
                Active = active,
                CarBrandId = carBrandId,
                ProviderId = providerId,
                YearRelease = yearRelease,
                BlockNumber = blockNumber,
                BlockTypeId = blockTypeId,
            };

            var result = new FileModel
            {
                Name = $"Транспортные средства {DateTime.Now.ToDateTimeString()}",
                FileFormat = fileFormat
            };

            if (fileFormat == FileFormatEnum.Pdf)
            {
                var htmlString = await _htmlBuilder.GetObjectsTable(printFile);
                result.Bytes = _pdfService.ConvertHtmlToPdf(htmlString);
            }
            else if (fileFormat == FileFormatEnum.Xlsx)
            {
                result.Bytes = _xlsxBuilder.GetObjectsTable(printFile);
            }
            else
            {
                throw new NotImplementedException();
            }

            return result;
        }

        public async Task<ObjectModel> GetByIdAsync(int id)
        {
            var result = await _objectRepository.GetPureByIdAsync(id);
            return MapToModel<ObjectModel>(result);
        }
        
        public async Task<ObjectModel> ChangeRouteAsync(
            int id, 
            int newRouteId,
            ClaimsPrincipal principal,
            bool updateBusRoutes)
        {
            var userRoutesModel = await _userManager.GetAvailableRoutesModel(principal);
            var entity = await _objectRepository.GetByIdAsync(id);
            var oldEntity = entity.Clone();

            if (userRoutesModel.ProjectId.HasValue && userRoutesModel.ProjectId != entity.ProjectId)
            {
                throw new InvalidOperationException("Invalid user with invalid project id");
            }

            if (userRoutesModel.RouteIds != null && !(entity.LastRouteId.HasValue && userRoutesModel.RouteIds.Contains(entity.LastRouteId.Value)))
            {
                throw new InvalidOperationException("Invalid user with invalid route ids");
            }

            if (entity.ObjectOutput)
            {
                throw new InvalidOperationException("Object should be active");
            }

            var project = await _projectRepository.GetProjectByRouteIdAsync(newRouteId);

            entity.LastRouteId = newRouteId;
            entity.ProjectId = project.Id;

            var updatedEntity = await _objectRepository.UpdateAsync(entity);

            await _eventLogCreator.CreateLog(principal, EventEnum.ChangeObjectRoute, oldEntity, updatedEntity);

            if (updateBusRoutes)
            {
                await _busDataRepository.UpdateBusRoutes(updatedEntity);
            }

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

            if (!entity.ObjectOutput)
            {
                throw new InvalidOperationException("Object should be disabled");
            }

            var project = await _projectRepository.GetProjectByRouteIdAsync(newRouteId);

            entity.LastRouteId = newRouteId;
            entity.ProjectId = project.Id;
            entity.ObjectOutput = false;
            
            if (await _objectRepository.AnyByObjIdProjIdAsync(entity.ObjId, entity.ProjectId, entity.Id))
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

            if (entity.ObjectOutput)
            {
                throw new InvalidOperationException("Object should be active");
            }

            entity.ObjectOutput = true;
            entity.ObjectOutputDate = DateTime.Now;

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
                ObjectOutput = true,
                DateInserted = DateTime.Now,
                ObjectOutputDate = DateTime.Now
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
            
            if (entity.LastRouteId != request.RouteId)
            {
                entity.LastRouteId = request.RouteId;

                if (entity.LastRouteId.HasValue)
                {
                    var project = await _projectRepository.GetProjectByRouteIdAsync(entity.LastRouteId.Value);
                    entity.ProjectId = project.Id;
                }
                else
                {
                    entity.ProjectId = 0;
                }
            }

            if (entity.ObjId == 0)
            {
                entity.ObjId = await _objectRepository.GetNextObjectIdAsync();
            }
            else
            {
                var entityId = entity.Id > 0 ? entity.Id : (int?)null;
                var needUpdateObjId = await _objectRepository.AnyByObjIdProjIdAsync(entity.ObjId, entity.ProjectId, entityId);

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
