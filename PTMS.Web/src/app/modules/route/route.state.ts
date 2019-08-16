import { Injectable } from '@angular/core';
import { StoreConfig } from '@datorama/akita';
import { RouteFullDto } from '@app/core/dtos';
import { AppEntityStore, AppEntityState, AppQueryEntity } from '@app/core/akita-extensions';

export interface RouteState extends AppEntityState<RouteFullDto> { }

@Injectable()
@StoreConfig({
  name: 'route-page',
  resettable: true
})
export class RouteStore extends AppEntityStore<RouteState, RouteFullDto> {
  constructor() {
    super();
  }
}

@Injectable()
export class RouteQuery extends AppQueryEntity<RouteState, RouteFullDto> {
  constructor(protected store: RouteStore) {
    super(store);
  }
}
