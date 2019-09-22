using System;

namespace PTMS.DataServices.Infrastructure
{
    public class DataChangeEvent
    {
        public DataChangeEvent(Type entityType)
        {
            EntityType = entityType;
        }

        public Type EntityType { get; set; }
    }

    public delegate void OnDataChange(DataChangeEvent changeEvent);
}
