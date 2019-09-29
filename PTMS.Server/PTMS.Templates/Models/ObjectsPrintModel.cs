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

        public List<string> HeadColumns
        {
            get
            {
                var result = new List<string>();

                result.Add("Номер");
                result.Add("Маршрут");

                if (ShowProjects)
                {
                    result.Add("Перевозчик");
                }

                if (ShowProviders)
                {
                    result.Add("Установщик");
                }

                result.Add("Время посл.отклика");

                result.Add("Время посл.остановки");

                if (ShowBlocks)
                {
                    result.Add("Телефон(IMEI)");
                    result.Add("Блок");
                }

                result.Add("Марка ТС");

                if (ShowCarType)
                {
                    result.Add("Тип ТС");
                }

                result.Add("Год выпуска");

                result.Add("Статус");

                return result;
            }
        }

        public List<List<string>> TableColumns
        {
            get
            {
                var result = new List<List<string>>();

                foreach (var vehicle in Vehicles)
                {
                    var vehicleRow = new List<string>();

                    vehicleRow.Add(vehicle.Name);
                    vehicleRow.Add(vehicle.Route?.Name ?? Utils.NaString);

                    if (ShowProjects)
                    {
                        vehicleRow.Add(vehicle.Project?.Name ?? Utils.NaString);
                    }

                    if (ShowProviders)
                    {
                        vehicleRow.Add(vehicle.Provider?.Name ?? Utils.NaString);
                    }

                    vehicleRow.Add(vehicle.LastTime.ToDateTimeString());

                    vehicleRow.Add(vehicle.LastStationTime.ToDateTimeString());

                    if (ShowBlocks)
                    {
                        vehicleRow.Add(vehicle.Phone > 0 ? vehicle.Phone.ToString() : Utils.NaString);

                        if (vehicle.Block != null)
                        {
                            vehicleRow.Add(vehicle.Block.BlockType?.Name + " " + vehicle.Block.BlockNumber);
                        }
                        else
                        {
                            vehicleRow.Add(Utils.NaString);
                        }
                    }

                    vehicleRow.Add(vehicle.CarBrand?.Name ?? Utils.NaString);

                    if (ShowCarType)
                    {
                        vehicleRow.Add(vehicle.CarBrand?.CarType?.Name ?? Utils.NaString);
                    }

                    vehicleRow.Add(vehicle.YearRelease > 0 ? vehicle.YearRelease.ToString() : Utils.NaString);

                    vehicleRow.Add(vehicle.StatusName);

                    result.Add(vehicleRow);
                }

                return result;
            }
        }

        public ObjectsPrintModel(List<Objects> vehicles, string roleName)
        {
            Vehicles = vehicles;
            Role = roleName;
        }
    }
}
