﻿using System.ComponentModel.DataAnnotations;

namespace PTMS.BusinessLogic.Models
{
    public class ObjectAddEditRequest
    {
        [Required]
        public string Name { get; set; }
        public short ProviderId { get; set; }
        public int? CarBrandId { get; set; }
        [Required]
        public long Phone { get; set; }
        public int? YearRelease { get; set; }
        public int? BlockNumber { get; set; }
        public int? BlockTypeId { get; set; }
    }
}