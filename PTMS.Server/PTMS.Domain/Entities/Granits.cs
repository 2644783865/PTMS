﻿namespace PTMS.Domain.Entities
{
    public class Granit
    {
        public int Id { get; set; }
        public int? BlockNumber { get; set; }
        public int? BlockTypeId { get; set; }
        public int ObjectId { get; set; }

        public Objects Object { get; set; }
        public BlockType BlockType { get; set; }
    }
}
