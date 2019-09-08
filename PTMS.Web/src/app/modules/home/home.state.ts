import { Injectable } from '@angular/core';
import { StoreConfig } from '@datorama/akita';
import { AppEntityState, AppEntityStore, AppQueryEntity } from '@app/core/akita-extensions/app-entity-state';
import { switchMap } from 'rxjs/operators';
import { of, combineLatest, Observable } from 'rxjs';
import { ObjectDto, ProjectDto, ProviderDto, RouteDto, PlanByRouteDto } from '@app/core/dtos';
import { EventLogDto } from '@app/core/dtos/EventLogDto';

export interface HomeState extends AppEntityState<ObjectDto> {
  projects: ProjectDto[];
  providers: ProviderDto[];
  routes: RouteDto[];
  plansByRoutes: PlanByRouteDto[];
  eventLogs: EventLogDto[];
  routeStatFilters: {
    showOnlyErrors: boolean,
    projectId: number,
    intervalId: string
  }
}

export interface ProjectStat {
  project: ProjectDto,
  factNumber: number,
  plannedNumber: number,
  onlineNumber: number,
  hasError: boolean
  hasWarning: boolean
}

export interface RouteStat {
  route: RouteDto,
  factNumber: number,
  plannedNumber: number,
  onlineNumber: number,
  hasError: boolean
  hasWarning: boolean
}

export interface ProviderStat {
  provider: ProviderDto,
  onlineNumber: number
}

@Injectable()
@StoreConfig({
  idKey: 'id',
  name: 'home',
  resettable: true
})
export class HomeStore extends AppEntityStore<HomeState, ObjectDto> {
  
  setRelatedData(
    projects: ProjectDto[],
    providers: ProviderDto[],
    routes: RouteDto[],
    plansByRoutes: PlanByRouteDto[]) {

    this.update({
      projects,
      providers,
      routes,
      plansByRoutes
    });
  }

  setRouteStatFilters(routeStatFilters) {
    this.update({
      routeStatFilters
    });
  }

  setEventLogs(eventLogs: EventLogDto[]){
    this.update({
      eventLogs
    })
  }

  constructor() {
    let initialState = {
      projects: [],
      providers: [],
      routes: [],
      plansByRoutes: [],
      eventLogs: [],
      routeStatFilters: {
        showOnlyErrors: false,
        projectId: null,
        intervalId: 'today'
      }
    } as HomeState;

    super(initialState);
  }
}

@Injectable()
export class HomeQuery extends AppQueryEntity<HomeState, ObjectDto> {
  projects$ = this.select(s => s.projects);
  providers$ = this.select(s => s.providers);
  routes$ = this.select(s => s.routes);
  eventLogs$ = this.select(s => s.eventLogs);

  statByProject$: Observable<ProjectStat[]> = combineLatest(
    this.list$,
    this.select(s => s.projects),
    this.select(s => s.plansByRoutes),
    this.select(s => s.routeStatFilters)
  ).pipe(
    switchMap(([objects, projects, plansByRoutes, routeStatFilters]) => {
      let result = projects.map(project => {
        let filteredPlans = plansByRoutes.filter(x => x.projectId == project.id);
        let plannedNumber = filteredPlans.reduce((sum, x) => sum + x.plannedNumber, 0);
        let factNumber = filteredPlans.reduce((sum, x) => sum + x.factNumber, 0);

        let item: ProjectStat = {
          project,
          factNumber: filteredPlans.length > 0 ? factNumber : undefined,
          plannedNumber: filteredPlans.length > 0 ? plannedNumber : undefined,
          onlineNumber: objects.filter(x => x.projId == project.id).length,
          hasError: false,
          hasWarning: false
        };

        item.hasError = item.onlineNumber < item.factNumber;
        item.hasWarning = item.onlineNumber > item.plannedNumber;

        return item;
      });

      if (routeStatFilters.showOnlyErrors) {
        result = result.filter(x => x.hasError || x.hasWarning);
      }

      if (routeStatFilters.projectId) {
        result = result.filter(x => x.project.id == routeStatFilters.projectId);
      }

      return of(result);
    })
  );

  statByProvider$: Observable<ProviderStat[]> = combineLatest(
    this.list$,
    this.select(s => s.providers),
    this.select(s => s.plansByRoutes),
    this.select(s => s.routeStatFilters)
  ).pipe(
    switchMap(([objects, providers, plansByRoutes, routeStatFilters]) => {
      if (routeStatFilters.projectId) {
        let routeIds = plansByRoutes.filter(x => x.projectId == routeStatFilters.projectId).map(x => x.routeId);
        objects = objects.filter(x => routeIds.includes(x.lastRout));
      }

      let result = providers.map(provider => {
        let item: ProviderStat = {
          provider,
          onlineNumber: objects.filter(x => x.providerId == provider.id).length
        };

        return item;
      });

      return of(result);
    })
  );
  
  statByRoute$: Observable<RouteStat[]> = combineLatest(
    this.list$,
    this.select(s => s.routes),
    this.select(s => s.plansByRoutes),
    this.select(s => s.routeStatFilters)
  ).pipe(
    switchMap(([objects, routes, plansByRoutes, routeStatFilters]) => {
      let result = routes.map(route => {
        let planByRoute = plansByRoutes.find(x => x.routeId == route.id);

        let item: RouteStat = {
          route,
          factNumber: planByRoute ? planByRoute.factNumber : undefined,
          plannedNumber: planByRoute ? planByRoute.plannedNumber : undefined,
          onlineNumber: objects.filter(x => x.lastRout == route.id).length,
          hasError: false,
          hasWarning: false
        };

        item.hasError = item.onlineNumber < item.factNumber;
        item.hasWarning = item.onlineNumber > item.plannedNumber;

        return item;
      });

      if (routeStatFilters.showOnlyErrors) {
        result = result.filter(x => x.hasError || x.hasWarning);
      }

      if (routeStatFilters.projectId) {
        let routeIds = plansByRoutes.filter(x => x.projectId == routeStatFilters.projectId).map(x => x.routeId);
        result = result.filter(x => routeIds.includes(x.route.id));
      }

      return of(result);
    })
  );

  totalOnline$: Observable<number> = this.statByRoute$
    .pipe(
      switchMap(stats => {
        let result = stats.reduce((sum, item) => sum + item.onlineNumber, 0);
        return of(result);
      })
    );

  totalPlanned$: Observable<number> = this.statByRoute$
    .pipe(
      switchMap(stats => {
        let result = stats.reduce((sum, item) => sum + (item.plannedNumber || 0), 0);
        return of(result);
      })
    );

  totalFact$: Observable<number> = this.statByRoute$
    .pipe(
      switchMap(stats => {
        let result = stats.reduce((sum, item) => sum + (item.factNumber || 0), 0);
        return of(result);
      })
    );

  constructor(protected store: HomeStore) {
    super(store);
  }
}
