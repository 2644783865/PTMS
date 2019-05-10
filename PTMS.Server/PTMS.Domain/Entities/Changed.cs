using System;
using System.Collections.Generic;

namespace PTMS.Domain.Entities
{
    public class Changed
    {
        public string TableName { get; set; }
        public string User { get; set; }
        public DateTime Time { get; set; }
        public string FieldName { get; set; }
        public string OldFieldValue { get; set; }
        public string NewFieldValue { get; set; }
        public int TypeChaged { get; set; }
        public int Ids { get; set; }
        public int ChgIds { get; set; }
    }
}
