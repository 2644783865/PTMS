namespace PTMS.BusinessLogic.Models
{
    public class GranitModel
    {
        public int Id { get; set; }
        public int? BlockNumber { get; set; }
        public int? BlockTypeId { get; set; }
        public int ObjectId { get; set; }

        public BlockTypeModel BlockType { get; set; }
    }
}
