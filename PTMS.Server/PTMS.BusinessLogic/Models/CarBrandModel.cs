namespace PTMS.BusinessLogic.Models
{
    public class CarBrandModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? CarTypeId { get; set; }
        public string L { get; set; }
        public string W { get; set; }
        public string H { get; set; }
        public CarTypeModel CarType { get; set; }
    }
}
