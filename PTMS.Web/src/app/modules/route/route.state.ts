import { Injectable } from '@angular/core';
import { EntityState, EntityStore, StoreConfig, QueryEntity } from '@datorama/akita';
import { RouteDto } from '@app/core/dtos';

export interface RouteState extends EntityState<RouteDto> { }

@Injectable()
@StoreConfig({
  name: 'route-page',
  resettable: true
})
export class RouteStore extends EntityStore<RouteState, RouteDto> {
  constructor() {
    super();
  }
}

@Injectable()
export class RouteQuery extends QueryEntity<RouteState, RouteDto> {
  constructor(protected store: RouteStore) {
    super(store);
  }
}
