using PTMS.Common;
using PTMS.Common.Enums;
using PTMS.DataServices.Infrastructure;
using PTMS.DataServices.IRepositories;
using PTMS.Domain.Entities;
using PTMS.Domain.Enums;
using PTMS.Persistance;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PTMS.DataServices.Repositories
{
    public class EventLogRepository : DataServiceAsync<EventLog>, IEventLogRepository
    {
        private readonly string[] _includes = new[]
        {
            nameof(EventLog.User),
            nameof(EventLog.User) + "." + nameof(AppUser.UserRoles),
            nameof(EventLog.EventLogFields)
        };

        public EventLogRepository(ApplicationDbContext context)
            : base(context)
        {

        }

        public Task<PageResult<EventLog>> FindByParamsAsync(
            string entityType,
            int? entityId,
            string entityName,
            EventEnum? eventEnum,
            int? userId,
            DateTime? startDate,
            DateTime? endDate,
            string fieldName,
            string fieldValue,
            bool onlyProject,
            OrderByEnum orderBy,
            int? page,
            int? pageSize)
        {
            Expression<Func<EventLog, bool>> filter = x =>
                (string.IsNullOrEmpty(entityType) || x.EntityType == entityType)
                && (string.IsNullOrEmpty(entityName) || x.EntityName == entityName)
                && (!entityId.HasValue || x.EntityId == entityId.Value)
                && (!eventEnum.HasValue || x.Event == eventEnum.Value)
                && (!userId.HasValue || x.UserId == userId.Value)
                && (!startDate.HasValue || x.TimeStamp >= startDate.Value)
                && (!endDate.HasValue || x.TimeStamp <= endDate.Value)
                && (string.IsNullOrEmpty(fieldName) || x.EventLogFields.Any(y => y.FieldName == fieldName))
                && (string.IsNullOrEmpty(fieldValue) || x.EventLogFields.Any(y => y.OldFieldValue == fieldValue || y.NewFieldValue == fieldValue))
                && (!onlyProject || x.User.ProjectId > 0);

            return FindPagedAsync(
                filter,
                x => x.TimeStamp,
                orderBy,
                page,
                pageSize,
                _includes);
        }
    }
}
