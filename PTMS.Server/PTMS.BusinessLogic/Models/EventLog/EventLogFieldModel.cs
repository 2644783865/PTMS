namespace PTMS.BusinessLogic.Models.EventLog
{
    public class EventLogFieldModel
    {
        public int Id { get; set; }
        public int EventLogId { get; set; }
        public string FieldName { get; set; }
        public string OldFieldValue { get; set; }
        public string NewFieldValue { get; set; }
    }
}
