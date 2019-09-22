namespace PTMS.DataServices.Infrastructure
{
    public class DataChangeEventEmitter : IDataChangeEventEmitter
    {
        private OnDataChange DataChangeSubscribers;

        public void Subscribe(OnDataChange func)
        {
            DataChangeSubscribers += func;
        }

        public void OnNext(DataChangeEvent changeEvent)
        {
            DataChangeSubscribers(changeEvent);
        }
    }
}
