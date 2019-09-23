namespace PTMS.BusinessLogic.Models.Shared
{
    public class TResultModel<T>
    {
        public TResultModel(T value)
        {
            Result = value;
        }

        public T Result { get; set; }
    }
}
