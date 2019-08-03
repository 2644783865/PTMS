using PTMS.Common;
using System;

namespace PTMS.BusinessLogic.Models
{
    public class ObjectModel
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
        public short? LastRout { get; set; }
        public short? VehicleType { get; set; }
        public short? Azmth { get; set; }
        public short ProviderId { get; set; }
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
        public string StatusName
        {
            get
            {
                if (ObjOutput)
                {
                    if (ObjOutputDate.HasValue)
                    {
                        return $"Выведено {ObjOutputDate.Value.ToDateString()}";
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

        public CarBrandModel CarBrand { get; set; }
        public ProviderModel Provider { get; set; }
        public RouteModel Route { get; set; }
        public ProjectModel Project { get; set; }
        public GranitModel Block { get; set; }
    }
}
