﻿using System;
using System.Collections.Generic;

namespace PTMS.Domain.Entities
{
    public class Plans
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

        public int TypePlannedTotal
        {
            get
            {
                var type1 = Type1Planned ?? 0;
                var type2 = Type2Planned ?? 0;
                var type3 = Type3Planned ?? 0;
                var type4 = Type4Planned ?? 0;

                return type1 + type2 + type3 + type4;
            }
        }
    }
}
