import { Injectable } from '@angular/core';
import { ObjectDto } from '@app/core/dtos/ObjectDto';
import { ProjectDto } from '@app/core/dtos/ProjectDto';
import { StoreConfig, SelectAllOptionsB, SelectAllOptionsD } from '@datorama/akita';
import { ProviderDto } from '@app/core/dtos/ProviderDto';
import { CarBrandDto } from '@app/core/dtos/CarBrandDto';
import { CarTypeDto } from '@app/core/dtos/CarTypeDto';
import { AppEntityState, AppEntityStore, AppQueryEntity } from '@app/core/akita-extensions/app-entity-state';
import { RouteDto } from '@app/core/dtos/RouteDto';
import { switchMap } from 'rxjs/operators';
import { of, combineLatest, Observable } from 'rxjs';

export interface HomeState extends AppEntityState<ObjectDto> {
  projects: ProjectDto[];
  providers: ProviderDto[];
  routes: RouteDto[];
}

export interface ProjectStat {
  project: ProjectDto,
  onlineNumber: number
}

export interface RouteStat {
  route: RouteDto,
  onlineNumber: number
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
    routes: RouteDto[]) {

    this.update({
      projects,
      providers,
      routes
    });
  }

  constructor() {
    let initialState = {
      projects: [],
      providers: [],
      routes: []
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
    this.select(s => s.projects)
  ).pipe(
    switchMap(([objects, projects]) => {
      let result = projects.map(project => {
        let item: ProjectStat = {
          project: project,
          onlineNumber: objects.filter(x => x.projId == project.id).length
        };

        return item;
      });

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

  statByRoute$: Observable<RouteStat[]> = combineLatest(
    this.list$,
    this.select(s => s.routes)
  ).pipe(
    switchMap(([objects, routes]) => {
      let result = routes.map(route => {
        let item: RouteStat = {
          route,
          onlineNumber: objects.filter(x => x.lastRout == route.id).length
        };

        return item;
      });

      return of(result);
    })
  );

  constructor(protected store: HomeStore) {
    super(store);
  }
}
