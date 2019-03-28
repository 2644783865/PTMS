using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PTMS.DataServices.Infrastructure
{
    public interface IDataServiceAsync<TEntity> : IDataServiceExAsync<TEntity, int>
        where TEntity : class
    {
    }

    public interface IDataServiceExAsync<TEntity, TPKey>
        where TEntity : class
    {
        Task<TEntity> GetByIdAsync(TPKey id);
        
        Task<TEntity> AddAsync(TEntity entity, bool commitChanges);

        Task<TEntity> UpdateAsync(TEntity entity, bool commitChanges);

        Task DeleteByIdAsync(TPKey id, bool commitChanges);
    }
}
