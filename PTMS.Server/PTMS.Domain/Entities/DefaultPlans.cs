using System;
using System.Collections.Generic;

namespace PTMS.Domain.Entities
{
    public class DefaultPlans
    {
        public int Ids { get; set; }
        public int? Rid { get; set; }
        public int? Pid { get; set; }
        public int? Type1Planned { get; set; }
        public int? Type2Planned { get; set; }
        public int? Type3Planned { get; set; }
        public int? Type4Planned { get; set; }
        public int? Race1Planned { get; set; }
        public int? Race2Planned { get; set; }
        public int? Race3Planned { get; set; }
        public int? Race4Planned { get; set; }
        public int? Type1PlannedHol { get; set; }
        public int? Type2PlannedHol { get; set; }
        public int? Type3PlannedHol { get; set; }
        public int? Type4PlannedHol { get; set; }
        public int? Race1PlannedHol { get; set; }
        public int? Race2PlannedHol { get; set; }
        public int? Race3PlannedHol { get; set; }
        public int? Race4PlannedHol { get; set; }
        public int? PlannedCount { get; set; }
        public int? PlannedCountHol { get; set; }
        public int? RaceCount { get; set; }
        public int? RaceCountHol { get; set; }

        public virtual Route Route { get; set; }
    }
}
