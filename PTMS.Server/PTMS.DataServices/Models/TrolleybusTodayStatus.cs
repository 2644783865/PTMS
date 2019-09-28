using System;

namespace PTMS.DataServices.Models
{
    public class TrolleybusTodayStatus
    {
        public string Name { get; set; }
        public string RouteName { get; set; }
        public string Place { get; set; }
        public DateTime? CoordTime { get; set; }
        public string NewTrolleyNumber
        {
            get
            {
                if (!string.IsNullOrEmpty(Place) && Place.Contains("-"))
                {
                    var array = Place.Split("-");

                    if (array.Length > 0)
                    {
                        return array[0];
                    }
                }

                return null;
            }
        }
    }
}
