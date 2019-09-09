import { Injectable } from '@angular/core';
import { RouteStore } from './route.state';
import { RouteDataService, ProjectDataService, BusStationDataService, BusStationRouteDataService } from '@app/core/data-services';
import { applyTransaction } from '@datorama/akita';
import { RouteFullDto, BusStationRouteDto, RouteDto } from '@app/core/dtos';
import { InlineFormResult, PromiseChainHelper } from '@app/core/helpers';
import { NotificationService } from '@app/core/notification';

export interface RouteSaveResult {
  success: boolean
  route: RouteDto
  busStationRoutes: BusStationRouteDto[]
}

@Injectable()
export class RouteService {
  constructor(
    private routeStore: RouteStore,
    private routeDataService: RouteDataService,
    private projectDataService: ProjectDataService,
    private busStationDataService: BusStationDataService,
    private busStationRouteDataService: BusStationRouteDataService,
    private notificationService: NotificationService)
  {
  }  

  async loadData() {
    this.routeStore.setLoading(true);

    let [projects, routes ] = await Promise.all([
      this.projectDataService.getAll(),
      this.routeDataService.getAllForPage()
    ]);
    
    routes.forEach(route => {
      if (route.projectId) {
        route.project = projects.find(p => p.id == route.projectId);
      }
    })

    applyTransaction(() => {
      this.routeStore.setProjects(projects);
      this.routeStore.set(routes);
      this.routeStore.setLoading(false);
    })
  }

  async loadDataForEdit(routeId: number | null) {
    this.routeStore.setModalLoading(true);

    let storeValue = this.routeStore.getValue();

    let [route, busStations, projects ] = await Promise.all([
      routeId ? this.routeDataService.getByIdForEdit(routeId) : Promise.resolve(null),
      !storeValue.busStations ? this.busStationDataService.getAll() : Promise.resolve(storeValue.busStations),
      !storeValue.projects ? this.projectDataService.getAll() : Promise.resolve(storeValue.projects)
    ]);

    applyTransaction(() => {
      this.routeStore.setBusStations(busStations);
      this.routeStore.setProjects(projects);
      this.routeStore.setModalLoading(false);
    })

    route = route || {
      routeActive: true,
      busStationRoutes: []
    } as RouteFullDto;

    return route;
  }

  async save(routeFormValue: any, busStationRoutesResult: InlineFormResult): Promise<RouteSaveResult> {
    let result = {success: true, busStationRoutes: []} as RouteSaveResult;

    try {
      this.routeStore.setModalLoading(true);

      let dto = {
        id: routeFormValue.id || 0,
        name: routeFormValue.name,
        projectId: routeFormValue.projectId,
        routeActive: routeFormValue.routeActive
      } as RouteFullDto;

      result.route = await this.routeDataService.addOrUpdate(dto);

      await this.saveBusStations(result, busStationRoutesResult);

      this.notificationService.success(`Маршрут ${result.route.name} был успешно ${dto.id > 0 ? 'отредактирован' : 'добавлен'}`, result.route.id);
    }
    catch (exc) {
      this.notificationService.exception(exc);
      result.success = false;
    }
    finally {
      this.routeStore.setModalLoading(false);
    }

    return result;
  }

  onDestroy() {
    this.routeStore.reset();
  }

  private async saveBusStations(result: RouteSaveResult, busStationRoute: InlineFormResult) {
    let routeId = result.route.id;

    let promiseChainer = new PromiseChainHelper();

    busStationRoute.toDelete.map(formValue => {
      promiseChainer.add(() => {
        return this.busStationRouteDataService.delete(formValue.id as number)
      });
    });

    busStationRoute.toUpdate.map(formValue => {
      let dto = {
        id: formValue.id,
        busStationId: formValue.busStation.id,
        routeId: routeId,
        num: formValue.num,
        isEndingStation: formValue.isEndingStation
      } as BusStationRouteDto;

      promiseChainer.add(() => {
        return this.busStationRouteDataService.update(dto)
      })
    });

    busStationRoute.toAdd.map(formValue => {
      let dto = {
        busStationId: formValue.busStation.id,
        routeId: routeId,
        num: formValue.num,
        isEndingStation: formValue.isEndingStation
      } as BusStationRouteDto;

      promiseChainer.add(() => {
        return this.busStationRouteDataService.add(dto);
      })
    });

    let stationsResult = await promiseChainer.run();

    result.busStationRoutes.push(...stationsResult.successResponses.filter(x => !!x));

    if (stationsResult.errors.length > 0){
      throw stationsResult.errors[0];
    }
  }
}
