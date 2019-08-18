import { Injectable } from '@angular/core';
import { StoreConfig } from '@datorama/akita';
import { RouteFullDto, ProjectDto, BusStationDto } from '@app/core/dtos';
import { AppEntityStore, AppEntityState, AppQueryEntity } from '@app/core/akita-extensions';

export interface RouteState extends AppEntityState<RouteFullDto> { 
  projects: ProjectDto[]
  busStations: BusStationDto[]
}

@Injectable()
@StoreConfig({
  name: 'route-page',
  resettable: true
})
export class RouteStore extends AppEntityStore<RouteState, RouteFullDto> {

  setProjects(projects: ProjectDto[]){
    this.update({ projects });
  }

  setBusStations(busStations: BusStationDto[]){
    this.update({ busStations });
  }

  constructor() {
    super();
  }
}

@Injectable()
export class RouteQuery extends AppQueryEntity<RouteState, RouteFullDto> {
  projects$ = this.select(x => x.projects);
  busStations$ = this.select(x => x.busStations);

  constructor(protected store: RouteStore) {
    super(store);
  }
}
