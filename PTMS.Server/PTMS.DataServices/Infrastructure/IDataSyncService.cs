using System.Collections.Generic;

namespace PTMS.DataServices.Infrastructure
{
    public interface IDataSyncServiceEx<TEntity, TPKey> where TEntity: class
    {
        void SyncOnAdd(TEntity entity);

        void SyncOnUpdate(TEntity entity);

        void SyncOnDelete(TPKey id);
    }
}
