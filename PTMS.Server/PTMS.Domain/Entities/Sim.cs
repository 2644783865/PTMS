namespace PTMS.Domain.Entities
{
    public class Sim
    {
        public int Id { get; set; }
        public long Phone { get; set; }
        public short? Status { get; set; }
        public string Comment { get; set; }
        public string Associated { get; set; }
    }
}
