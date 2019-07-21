using System;

namespace PTMS.BusinessLogic.Models.Plan
{
    public class PlanModel
    {
        public int Id { get; set; }
        public DateTime? Date { get; set; }
        public int? RoutId { get; set; }
        public int? Type1Planned { get; set; }
        public int? Type2Planned { get; set; }
        public int? Type3Planned { get; set; }
        public int? Type4Planned { get; set; }
        public int? Race1Planned { get; set; }
        public int? Race2Planned { get; set; }
        public int? Race3Planned { get; set; }
        public int? Race4Planned { get; set; }
        public int? ProjId { get; set; }
        public DateTime? DateModify { get; set; }
    }
}
