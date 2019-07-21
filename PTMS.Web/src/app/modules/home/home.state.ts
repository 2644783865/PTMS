import { Injectable } from '@angular/core';
import { ObjectDto } from '@app/core/dtos/ObjectDto';
import { ProjectDto } from '@app/core/dtos/ProjectDto';
import { StoreConfig } from '@datorama/akita';
import { ProviderDto } from '@app/core/dtos/ProviderDto';
import { AppEntityState, AppEntityStore, AppQueryEntity } from '@app/core/akita-extensions/app-entity-state';
import { RouteDto } from '@app/core/dtos/RouteDto';
import { switchMap } from 'rxjs/operators';
import { of, combineLatest, Observable } from 'rxjs';
import { PlanByRouteDto } from '@app/core/dtos/PlanByRouteDto';

export interface HomeState extends AppEntityState<ObjectDto> {
  projects: ProjectDto[];
  providers: ProviderDto[];
  routes: RouteDto[];
  plansByRoutes: PlanByRouteDto[];
  routeStatFilters: {
    showOnlyErrors: boolean,
    projectId: number,
    intervalId: string
  }
}

export interface ProjectStat {
  project: ProjectDto,
  plannedNumber: number,
  onlineNumber: number,
  hasError: boolean
}

export interface RouteStat {
  route: RouteDto,
  plannedNumber: number,
  onlineNumber: number,
  hasError: boolean
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

  constructor() {
    let initialState = {
      projects: [],
      providers: [],
      routes: [],
      plansByRoutes: [],
      routeStatFilters: {
        showOnlyErrors: false,
        projectId: null,
        intervalId: '10'
      }
    } as HomeState;

    super(initialState);
  }
}

@Injectable()
export class HomeQuery extends AppQueryEntity<HomeState, ObjectDto> {
  private list$: Observable<ObjectDto[]> = this.selectAll();

  projects$ = this.select(s => s.projects);
  providers$ = this.select(s => s.providers);
  routes$ = this.select(s => s.routes);

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

        let item: ProjectStat = {
          project,
          plannedNumber: filteredPlans.length > 0 ? plannedNumber : undefined,
          onlineNumber: objects.filter(x => x.projId == project.id).length,
          hasError: false
        };

        item.hasError = item.onlineNumber != item.plannedNumber;

        return item;
      });

      if (routeStatFilters.showOnlyErrors) {
        result = result.filter(x => x.hasError);
      }

      if (routeStatFilters.projectId) {
        result = result.filter(x => x.project.id == routeStatFilters.projectId);
      }

      return of(result);
    })
  );

  totalOnlineByProject$: Observable<number> = this.statByProject$
    .pipe(
      switchMap(stats => {
        let result = stats.reduce((sum, item) => sum + item.onlineNumber, 0);
        return of(result);
      })
    );

  totalPlannedByProject$: Observable<number> = this.statByProject$
    .pipe(
      switchMap(stats => {
        let result = stats.reduce((sum, item) => sum + (item.plannedNumber || 0), 0);
        return of(result);
      })
    );

  statByProvider$: Observable<ProviderStat[]> = combineLatest(
    this.list$,
    this.select(s => s.providers)
  ).pipe(
    switchMap(([objects, providers]) => {
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

  totalOnlineByProvider$: Observable<number> = this.statByProvider$
    .pipe(
      switchMap(stats => {
        let result = stats.reduce((sum, item) => sum + item.onlineNumber, 0);
        return of(result);
      })
    );

  totalPlannedByProvider$: Observable<number> = this.select(s => s.plansByRoutes)
    .pipe(
      switchMap(plansByRoutes => {
        let result = plansByRoutes.reduce((sum, item) => sum + (item.plannedNumber || 0), 0);
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
          plannedNumber: planByRoute ? planByRoute.plannedNumber : undefined,
          onlineNumber: objects.filter(x => x.lastRout == route.id).length,
          hasError: false
        };

        item.hasError = item.onlineNumber != item.plannedNumber;

        return item;
      });

      if (routeStatFilters.showOnlyErrors) {
        result = result.filter(x => x.hasError);
      }

      if (routeStatFilters.projectId) {
        let routeIds = plansByRoutes.filter(x => x.projectId == routeStatFilters.projectId).map(x => x.routeId);
        result = result.filter(x => routeIds.includes(x.route.id));
      }

      return of(result);
    })
  );

  totalOnlineByRoute$: Observable<number> = this.statByRoute$
    .pipe(
      switchMap(stats => {
        let result = stats.reduce((sum, item) => sum + item.onlineNumber, 0);
        return of(result);
      })
    );

  totalPlannedByRoute$: Observable<number> = this.statByRoute$
    .pipe(
      switchMap(stats => {
        let result = stats.reduce((sum, item) => sum + (item.plannedNumber || 0), 0);
        return of(result);
      })
    );

  constructor(protected store: HomeStore) {
    super(store);
  }
}
