import { Injectable } from '@angular/core';
import { EntityState, EntityStore, StoreConfig, QueryEntity } from '@datorama/akita';
import { VehicleDto } from '@app/core/dtos/VehicleDto';
import { createPaginator } from '@app/core/paginator/app-paginator.token';

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

export const CHANGE_ROUTE_PAGINATOR = createPaginator<VehicleDto>('CHANGE_ROUTE_PAGINATOR', ChangeRouteQuery);
