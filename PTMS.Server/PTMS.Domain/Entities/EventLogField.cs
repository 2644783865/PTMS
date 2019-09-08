namespace PTMS.Domain.Entities
{
    public class EventLogField
    {
        public int Id { get; set; }
        public int EventLogId { get; set; }
        public string FieldName { get; set; }
        public string OldFieldValue { get; set; }
        public string NewFieldValue { get; set; }
        public EventLog EventLog { get; set; }
    }
}
