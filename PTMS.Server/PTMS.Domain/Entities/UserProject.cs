using System;
using System.Collections.Generic;

namespace PTMS.Domain.Entities
{
    public class UserProject
    {
        public int UserId { get; set; }
        public int Id { get; set; }
        public int ProjectId { get; set; }
    }
}
