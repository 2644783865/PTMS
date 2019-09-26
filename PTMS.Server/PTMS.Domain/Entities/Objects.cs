using System;

namespace PTMS.Domain.Entities
{
    public class Objects
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public short ObjId { get; set; }
        public DateTime? LastTime { get; set; }
        public double? LastLongitude { get; set; }
        public double? LastLatitude { get; set; }
        public short? LastSpeed { get; set; }
        public int ProjectId { get; set; }
        public short? LastStationId { get; set; }
        public DateTime? LastStationTime { get; set; }
        public int? LastRouteId { get; set; }
        public int? CarTypeId { get; set; }
        public int? Azimuth { get; set; }
        public int ProviderId { get; set; }
        public int? CarBrandId { get; set; }
        public string UserComment { get; set; }
        public DateTime? DateInserted { get; set; }
        public bool ObjectOutput { get; set; }
        public DateTime? ObjectOutputDate { get; set; }
        public long Phone { get; set; }
        public int? YearRelease { get; set; }
        public int? DispRouteId { get; set; }
        public short? LastAddInfo { get; set; }
        public bool? Lowfloor { get; set; }

        public string StatusName
        {
            get
            {
                if (ObjectOutput)
                {
                    if (ObjectOutputDate.HasValue)
                    {
                        return $"Выведено {ObjectOutputDate.Value.ToString("dd.MM.yyyy")}";
                    }
                    else
                    {
                        return "Выведено из эксплуатации";
                    }
                }
                else
                {
                    return "Активно";
                }
            }
        }

        public CarBrand CarBrand { get; set; }
        public Provider Provider { get; set; }
        public Project Project { get; set; }
        public Route Route { get; set; }
        public Granit Block { get; set; }

        public Objects Clone()
        {
            return (Objects)this.MemberwiseClone();
        }
    }
}
