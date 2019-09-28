using AutoMapper;
using PTMS.BusinessLogic.IServices;
using PTMS.BusinessLogic.Models;
using PTMS.BusinessLogic.Models.Dispatch;
using PTMS.BusinessLogic.Models.Object;
using PTMS.DataServices.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PTMS.BusinessLogic.Services
{
    public class DispatchService : IDispatchService
    {
        private readonly IBusDataRepository _busDataRepository;
        private readonly IRouteRepository _routeRepository;
        private readonly IObjectRepository _objectRepository;
        private readonly IMapper _mapper;

        public DispatchService(
            IBusDataRepository busDataRepository,
            IRouteRepository routeRepository,
            IObjectRepository objectRepository,
            IMapper mapper)
        {
            _busDataRepository = busDataRepository;
            _routeRepository = routeRepository;
            _mapper = mapper;
            _objectRepository = objectRepository;
        }

        public async Task<List<TrolleybusTodayStatusModel>> GetTrolleybusTodayStatus()
        {
            var routes = await _routeRepository.GetAllAsync(null, null, null);
            var statuses = await _busDataRepository.GetTrolleybusTodayStatus();
            var trolleybuses = await _objectRepository.GetAllTrolleybuses();

            var compType = StringComparison.InvariantCultureIgnoreCase;

            var result = statuses.Select(s =>
            {
                var currentRoute = routes.First(x => x.Name.Equals(s.RouteName, compType));
                var newRoute = !string.IsNullOrEmpty(s.NewTrolleyNumber)
                    ? routes.First(x => x.IsTrolleybus && Regex.Match(x.Name, @"\d+").Value.Equals(s.NewTrolleyNumber, compType))
                    : null;

                if (currentRoute == newRoute)
                {
                    newRoute = null;
                }

                var vehicle = trolleybuses.First(x => x.Name.Equals(s.Name, compType));
                vehicle.Route = routes.First(x => x.Id == vehicle.LastRouteId.Value);

                var item = new TrolleybusTodayStatusModel
                {
                    Place = s.Place,
                    CoordinationTime = s.CoordTime,
                    Trolleybus = _mapper.Map<ObjectModel>(vehicle),
                    NewRoute = _mapper.Map<RouteModel>(newRoute),
                    IsNotDefined = string.IsNullOrEmpty(s.NewTrolleyNumber)
                };

                return item;
            })
            .ToList();

            result.Sort((a, b) =>
            {
                var compareResult = -(a.NewRoute != null).CompareTo(b.NewRoute != null);

                if (compareResult == 0)
                {
                    compareResult = a.IsNotDefined.CompareTo(b.IsNotDefined);

                    if (compareResult == 0)
                    {
                        var aRouteNumber = int.Parse(Regex.Match(a.Trolleybus.Route.Name, @"\d+").Value);
                        var bRouteNumber = int.Parse(Regex.Match(b.Trolleybus.Route.Name, @"\d+").Value);

                        compareResult = aRouteNumber.CompareTo(bRouteNumber);

                        if (compareResult == 0)
                        {
                            compareResult = a.Trolleybus.Name.CompareTo(b.Trolleybus.Name);
                        }
                    }
                }

                return compareResult;
            });

            return result;
        }
    }
}
