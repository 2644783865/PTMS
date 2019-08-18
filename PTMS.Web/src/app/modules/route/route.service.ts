import { Injectable } from '@angular/core';
import { RouteStore } from './route.state';
import { RouteDataService, ProjectDataService, BusStationDataService, BusStationRouteDataService } from '@app/core/data-services';
import { applyTransaction } from '@datorama/akita';
import { RouteFullDto, BusStationRouteDto } from '@app/core/dtos';
import { InlineFormResult } from '@app/core/helpers';
import { NotificationService } from '@app/core/notification';

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

  async save(routeFormValue: any, busStationRoutesResult: InlineFormResult) {
    try {
      this.routeStore.setModalLoading(true);

      let dto = {
        id: routeFormValue.id,
        projectId: routeFormValue.projectId,
        routeActive: routeFormValue.routeActive
      } as RouteFullDto;

      let route = await this.routeDataService.addOrUpdate(dto);

      await this.saveBusStations(route.id, busStationRoutesResult);

      this.notificationService.success(`Маршрут ${route.name} был успешно ${dto.id > 0 ? 'отредактирован' : 'добавлен'}`, route.id);
    }
    catch (exc) {
      this.notificationService.exception(exc);
    }
    finally {
      this.routeStore.setModalLoading(false);
    }
  }

  onDestroy() {
    this.routeStore.reset();
  }

  private async saveBusStations(routeId: number, busStationRoute: InlineFormResult) {
    await Promise.all(busStationRoute.toDelete.map(formValue => {
      return this.busStationRouteDataService.delete(formValue.id as number);
    }));

    await Promise.all(busStationRoute.toUpdate.map(formValue => {
      let dto = {
        id: formValue.id,
        busStationId: formValue.busStation.id,
        routeId: routeId,
        num: formValue.num
      } as BusStationRouteDto;

      return this.busStationRouteDataService.update(dto);
    }));

    await Promise.all(busStationRoute.toAdd.map(formValue => {
      let dto = {
        busStationId: formValue.busStation.id,
        routeId: routeId,
        num: formValue.num
      } as BusStationRouteDto;

      return this.busStationRouteDataService.add(dto);
    }));
  }
}
