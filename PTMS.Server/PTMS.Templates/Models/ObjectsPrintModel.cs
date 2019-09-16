using PTMS.Common;
using PTMS.Domain.Entities;
using System.Collections.Generic;

namespace PTMS.Templates.Models
{
    public class ObjectsPrintModel
    {
        public string Role { get; set; }
        public List<Objects> Vehicles { get; set; }

        public bool ShowProjects
        {
            get { return Role == RoleNames.Administrator || Role == RoleNames.Dispatcher; }
        }

        public bool ShowProviders
        {
            get { return Role == RoleNames.Administrator || Role == RoleNames.Dispatcher; }
        }

        public bool ShowBlocks
        {
            get { return Role == RoleNames.Administrator || Role == RoleNames.Dispatcher; }
        }

        public ObjectsPrintModel(List<Objects> vehicles, string roleName)
        {
            Vehicles = vehicles;
            Role = roleName;
        }
    }
}
