using Microsoft.Extensions.Logging;
using PTMS.DataServices.IRepositories;
using PTMS.Domain.Entities;
using PTMS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PTMS.BusinessLogic.Helpers
{
    public class EventLogCreator
    {
        private readonly IEventLogRepository _eventLogRepository;
        private readonly IEventLogFieldRepository _eventLogFieldRepository;

        private readonly IRouteRepository _routeRepository;
        private readonly AppUserManager _appUserManager;
        private readonly ILogger<EventLogCreator> _logger;

        public EventLogCreator(
            IEventLogRepository eventLogRepository,
            IEventLogFieldRepository eventLogFieldRepository,
            IRouteRepository routeRepository,
            AppUserManager appUserManager,
            ILogger<EventLogCreator> logger)
        {
            _eventLogRepository = eventLogRepository;
            _eventLogFieldRepository = eventLogFieldRepository;
            _routeRepository = routeRepository;
            _appUserManager = appUserManager;
            _logger = logger;
        }

        public async Task CreateLog(
            ClaimsPrincipal principal,
            EventEnum eventEnum,
            Objects oldVehicle, 
            Objects newVehicle)
        {
            await WrapCall(async () =>
            {
                var trackableProperties = new string[]
                {
                    nameof(Objects.ProjId),
                    nameof(Objects.ObjId),
                    nameof(Objects.CarTypeId),
                    nameof(Objects.Name),
                    nameof(Objects.CarBrandId),
                    nameof(Objects.ProviderId),
                    nameof(Objects.LastRout),
                    nameof(Objects.ObjOutput),
                    nameof(Objects.ObjOutputDate),
                    nameof(Objects.Phone),
                    nameof(Objects.YearRelease)
                };

                var fields = GetChangedFields(
                    oldVehicle,
                    newVehicle,
                    trackableProperties);

                if (fields.Any())
                {
                    var entityId = oldVehicle != null ? oldVehicle.Id : newVehicle.Id;
                    var entityType = nameof(Objects);

                    if ((fields.Count == 1 && fields[0].FieldName == nameof(Objects.LastRout))
                        || fields.Count == 2 && fields.Any(x => x.FieldName == nameof(Objects.LastRout)) && fields.Any(x => x.FieldName == nameof(Objects.ProjId)))
                    {
                        eventEnum = EventEnum.ChangeObjectRoute;
                    }

                    string message = null;

                    switch (eventEnum)
                    {
                        case EventEnum.Create:
                            message = $"ТС {newVehicle.Name} было добавлено";
                            break;
                        case EventEnum.Update:
                            message = $"ТС {newVehicle.Name} было отредактировано";
                            break;
                        case EventEnum.Delete:
                            message = $"ТС {oldVehicle.Name} было удалёно";
                            break;
                        case EventEnum.ChangeObjectRoute:
                            var oldRoute = await _routeRepository.GetByIdAsync(oldVehicle.LastRout.Value);
                            var newRoute = await _routeRepository.GetByIdAsync(newVehicle.LastRout.Value);
                            message = $"Маршрут ТС {newVehicle.Name} был изменён с {oldRoute.Name} на {newRoute.Name}";
                            break;
                        case EventEnum.EnableObject:
                            newRoute = await _routeRepository.GetByIdAsync(newVehicle.LastRout.Value);
                            message = $"ТС {newVehicle.Name} было введено в эксплуатацию. Маршрут - {newRoute.Name}";
                            break;
                        case EventEnum.DisableObject:
                            message = $"ТС {newVehicle.Name} было выведено из эксплуатации";
                            break;
                    }

                    await CreateEventLog(
                        principal,
                        eventEnum,
                        message,
                        entityType,
                        entityId,
                        fields);
                }
            });
        }

        private async Task WrapCall(Func<Task> action)
        {
            try
            {
                await action();
            }
            catch (Exception exc)
            {
                _logger.LogError(exc, exc.Message);
            }
        }

        private async Task CreateEventLog(
            ClaimsPrincipal principal,
            EventEnum eventEnum,
            string message,
            string entityType,
            int entityId,
            List<EventLogField> fields)
        {
            if (string.IsNullOrEmpty(message))
            {
                throw new NotImplementedException("Message is required field");
            }

            var userId = int.Parse(_appUserManager.GetUserId(principal));

            var eventLog = new EventLog
            {
                Event = eventEnum,
                EntityId = entityId,
                EntityType = entityType,
                TimeStamp = DateTime.Now,
                UserId = userId,
                Message = message
            };

            var newEventLog = await _eventLogRepository.AddAsync(eventLog);

            if (eventEnum != EventEnum.Delete)
            {
                foreach (var field in fields)
                {
                    field.EventLogId = newEventLog.Id;
                    await _eventLogFieldRepository.AddAsync(field);
                }
            }
        }

        private List<EventLogField> GetChangedFields<T>(
            T oldEntity, 
            T newEntity, 
            params string[] trackableProperties)
        {
            var result = new List<EventLogField>();
            var entityType = typeof(T);

            foreach (var property in entityType.GetProperties())
            {
                var propertyName = property.Name;

                if (trackableProperties.Contains(propertyName))
                {
                    var oldValue = oldEntity != null ? property.GetValue(oldEntity, null) : null;
                    var newValue = newEntity != null ? property.GetValue(newEntity, null) : null;

                    if (!object.Equals(oldValue, newValue))
                    {
                        var sOldValue = oldValue?.ToString();
                        var sNewValue = newValue?.ToString();

                        result.Add(new EventLogField
                        {
                            FieldName = propertyName,
                            OldFieldValue = sOldValue,
                            NewFieldValue = sNewValue
                        });
                    }
                }
            }

            return result;
        }
    }
}
