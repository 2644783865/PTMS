using System;

namespace PTMS.DataServices.Infrastructure
{
    public interface IDataChangeEventEmitter
    {
        void Subscribe(OnDataChange func);
        void OnNext(DataChangeEvent changeEvent);
    }
}
