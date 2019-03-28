using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PTMS.Common;
using PTMS.Persistance;

namespace PTMS.DataServices.Infrastructure
{
    public abstract class DataServiceAsync<TEntity> : DataServiceAsyncEx<TEntity, int>
        where TEntity : class, new()
    {
        public DataServiceAsync(ApplicationDbContext dbContext)
            : base(dbContext)
        {
        }
    }

    public abstract class DataServiceAsyncEx<TEntity, TPKey> : IDataServiceExAsync<TEntity, TPKey>
        where TEntity : class, new()
    {
        private readonly ApplicationDbContext _dbContext;

        public DataServiceAsyncEx(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private DbSet<TEntity> Set => _dbContext.Set<TEntity>();

        public Task<TEntity> GetByIdAsync(TPKey id)
        {
            return GetByIdHelperAsync(id);
        }

        protected Task<TEntity> GetByIdAsync(TPKey id, params string[] includes)
        {
            return GetByIdHelperAsync(id, includes);
        }

        protected Task<List<TEntity>> GetAllAsync(params string[] includes)
        {
            return Prepare(includes).ToListAsync();
        }

        protected Task<List<TEntity>> FindAsync(
            Expression<Func<TEntity, bool>> filter, 
            params string[] includes)
        {
            return Prepare(includes).Where(filter).ToListAsync();
        }

        protected async Task<PageResult<TEntity>> FindPagedAsync(
            Expression<Func<TEntity, bool>> filter,
            Expression<Func<TEntity, object>> orderBySelector,
            bool orderByAsc,
            int? page,
            int? pageSize,
            params string[] includes)
        {
            var query = Prepare(includes);
            if (filter != null)
            {
                query = query.Where(filter);
            }

            query = orderByAsc
                ? query.OrderBy(orderBySelector)
                : query.OrderByDescending(orderBySelector);

            var actualPage = page ?? 1;
            var actualPageSize = pageSize.HasValue && (pageSize > 0 && pageSize <= 50)
                ? pageSize.Value
                : 50;

            var skipAmount = (actualPage - 1) * actualPageSize;

            var list = await query
                .AsNoTracking()
                .Skip(skipAmount)
                .Take(actualPageSize)
                .ToListAsync();

            var totalCount = await Set.Where(filter).CountAsync();

            var result = new PageResult<TEntity>(list, totalCount);
            return result;
        }

        public virtual async Task<TEntity> AddAsync(TEntity entity, bool commitChanges)
        {
            var addResult = await Set.AddAsync(entity);

            if (commitChanges)
            {
                await _dbContext.SaveChangesAsync();
            }

            return addResult.Entity;
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity entity, bool commitChanges)
        {
            var updateResult = Set.Update(entity);

            if (commitChanges)
            {
                await _dbContext.SaveChangesAsync();
            }

            return updateResult.Entity;
        }

        public virtual async Task DeleteByIdAsync(TPKey id, bool commitChanges)
        {
            var item = await Set.FindAsync(id);

            if (item == null)
            {
                throw new KeyNotFoundException("Не удалось удалить элемент. Элемент не найден в базе.");
            }

            Set.Remove(item);

            if (commitChanges)
            {
                await _dbContext.SaveChangesAsync();
            }
        }

        private Task<TEntity> GetByIdHelperAsync(TPKey id, params string[] includes)
        {
            return Prepare(includes).FirstOrDefaultAsync(GetFindExpression(id));
        }

        private IQueryable<TEntity> Prepare(params string[] includes)
        {
            var query = Set
                .AsQueryable();

            query = includes.Aggregate(query, (s, i) => s.Include(i));

            return query;
        }

        private Expression<Func<TEntity, bool>> GetFindExpression(object keyValue)
        {
            var parameter = Expression.Parameter(typeof(TEntity), "e");
            const string IdMemberName = "Id";

            var propertyInfo = typeof(TEntity).GetProperty(
                IdMemberName,
                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (propertyInfo == null)
            {
                throw new MissingMemberException(typeof(TEntity).Name, IdMemberName);
            }

            var left = Expression.Property(parameter, propertyInfo);

            var right = Expression.Property(
                Expression.Constant(new { Id = keyValue }),
                IdMemberName);

            var expression = Expression.Equal(left, Expression.Convert(right, propertyInfo.PropertyType));

            return Expression.Lambda<Func<TEntity, bool>>(expression, parameter);
        }
    }
}
