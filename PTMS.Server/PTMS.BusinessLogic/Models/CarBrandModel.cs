namespace PTMS.BusinessLogic.Models
{
    public class CarBrandModel
    {
        public int CbId { get; set; }
        public string CbName { get; set; }
        public int? CarTypeId { get; set; }
        public string L { get; set; }
        public string W { get; set; }
        public string H { get; set; }
        public CarTypeModel CarType { get; set; }
    }
}
