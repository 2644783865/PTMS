using System.Collections.Generic;

namespace PTMS.DataServices.Models
{
    public class UserAvailableRoutes
    {
        public bool AvailableAll
        {
            get
            {
                return !ProjectId.HasValue && RouteIds == null;
            }
        }

        public int? ProjectId { get; set; }
        public List<int> RouteIds { get; set; }
    }
}
