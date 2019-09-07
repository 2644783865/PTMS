import { Injectable } from '@angular/core';
import { ObjectDto, RouteDto } from '@app/core/dtos';
import { StoreConfig } from '@datorama/akita';
import { AppEntityState, AppEntityStore, AppQueryEntity } from '@app/core/akita-extensions';

export interface ChangeRouteState extends AppEntityState<ObjectDto> {
  routes: RouteDto[]
}

@Injectable()
@StoreConfig({ name: 'change-route' })
export class ChangeRouteStore extends AppEntityStore<ChangeRouteState, ObjectDto> {

  setRelatedData(routes: RouteDto[]){
    this.update({routes});
  }

  constructor() {
    super();
  }
}

@Injectable()
export class ChangeRouteQuery extends AppQueryEntity<ChangeRouteState, ObjectDto> {
  routes$ = this.select(x => x.routes);

  constructor(protected store: ChangeRouteStore) {
    super(store);
  }
}
