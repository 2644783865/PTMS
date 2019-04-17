import { Injectable, inject, InjectionToken } from '@angular/core';
import { EntityState, EntityStore, StoreConfig, QueryEntity } from '@datorama/akita';
import { VehicleDto } from '@app/core/dtos/VehicleDto';
import { AppPaginator } from '@app/core/paginator/app-paginator.plugin';

export interface VehicleState extends EntityState<VehicleDto> { }

@Injectable()
@StoreConfig({ name: 'change-route' })
export class ChangeRouteStore extends EntityStore<VehicleState, VehicleDto> {
  constructor() {
    super();
  }
}

@Injectable()
export class ChangeRouteQuery extends QueryEntity<VehicleState, VehicleDto> {
  constructor(protected store: ChangeRouteStore) {
    super(store);
  }
}

export const CHANGE_ROUTE_PAGINATOR = new InjectionToken('CHANGE_ROUTE_PAGINATOR', {
  factory: () => {
    return new AppPaginator<VehicleDto>(inject(ChangeRouteQuery));
  }
});
