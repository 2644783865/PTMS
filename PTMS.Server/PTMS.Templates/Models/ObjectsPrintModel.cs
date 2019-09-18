using PTMS.Common;
using PTMS.Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace PTMS.Templates.Models
{
    public class ObjectsPrintModel
    {
        public string PlateNumber { get; set; }
        public string RouteName { get; set; }
        public long? CarTypeId { get; set; }
        public long? ProjectId { get; set; }
        public bool? Active { get; set; }
        public long? CarBrandId { get; set; }
        public long? ProviderId { get; set; }
        public long? YearRelease { get; set; }
        public string BlockNumber { get; set; }
        public long? BlockTypeId { get; set; }

        public CarType CarType => CarTypeId.HasValue
            ? Vehicles.FirstOrDefault(x => x.CarBrand?.CarType?.Id == CarTypeId.Value).CarBrand.CarType
            : null;

        public BlockType BlockType => BlockTypeId.HasValue
            ? Vehicles.FirstOrDefault(x => x.Block?.BlockType?.Id == BlockTypeId.Value).Block.BlockType
            : null;

        public Provider Provider => ProviderId.HasValue
            ? Vehicles.FirstOrDefault(x => x.Provider?.Id == ProviderId.Value).Provider
            : null;

        public Project Project => ProjectId.HasValue
           ? Vehicles.FirstOrDefault(x => x.Project?.Id == ProjectId.Value).Project
           : null;

        public CarBrand CarBrand => CarBrandId.HasValue
           ? Vehicles.FirstOrDefault(x => x.CarBrand?.Id == CarBrandId.Value).CarBrand
           : null;

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

        public bool ShowCarType
        {
            get { return Role == RoleNames.Administrator || Role == RoleNames.Dispatcher; }
        }

        public int FontSize
        {
            get { return Role == RoleNames.Administrator || Role == RoleNames.Dispatcher ? 12 : 14; }
        }

        public ObjectsPrintModel(List<Objects> vehicles, string roleName)
        {
            Vehicles = vehicles;
            Role = roleName;
        }
    }
}
