﻿using System;

namespace PTMS.Domain.Entities
{
    public class Objects
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public short ObjId { get; set; }
        public DateTime? LastTime { get; set; }
        public double? LastLon { get; set; }
        public double? LastLat { get; set; }
        public short? LastSpeed { get; set; }
        public int ProjId { get; set; }
        public short? LastStation { get; set; }
        public DateTime? LastStationTime { get; set; }
        public int? LastRout { get; set; }
        public int? CarTypeId { get; set; }
        public int? Azmth { get; set; }
        public int ProviderId { get; set; }
        public int? CarBrandId { get; set; }
        public string UserComment { get; set; }
        public DateTime? DateInserted { get; set; }
        public bool ObjOutput { get; set; }
        public DateTime? ObjOutputDate { get; set; }
        public long Phone { get; set; }
        public int? YearRelease { get; set; }
        public int? DispRoute { get; set; }
        public short? LastAddInfo { get; set; }
        public short? Lowfloor { get; set; }

        public CarBrand CarBrand { get; set; }
        public Provider Provider { get; set; }
        public Project Project { get; set; }
        public Route Route { get; set; }
        public Granit Block { get; set; }
    }
}
