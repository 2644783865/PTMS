using System;

namespace PTMS.Domain.Entities
{
    public class Objects
    {
        public string Name { get; set; }
        public short ObjId { get; set; }
        public DateTime? LastTime { get; set; }
        public double? LastLon { get; set; }
        public double? LastLat { get; set; }
        public short? LastSpeed { get; set; }
        public int ProjId { get; set; }
        public short? LastStation { get; set; }
        public DateTime? LastStationTime { get; set; }
        public short? LastRout { get; set; }
        public short? VehicleType { get; set; }
        public short? Azmth { get; set; }
        public int ProviderId { get; set; }
        public decimal Ids { get; set; }
        public int? CarBrandId { get; set; }
        public string UserComment { get; set; }
        public DateTime? DateInserted { get; set; }
        public int? ObjOutput { get; set; }
        public DateTime? ObjOutputDate { get; set; }
        public long Phone { get; set; }
        public int? YearRelease { get; set; }
        public int? DispRoute { get; set; }
        public short? LastAddInfo { get; set; }
        public short? Lowfloor { get; set; }

        public virtual CarBrand CarBrand { get; set; }
        public virtual Providers Provider { get; set; }
    }
}
