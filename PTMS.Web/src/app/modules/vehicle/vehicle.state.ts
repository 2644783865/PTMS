import { Injectable } from '@angular/core';
import { EntityState, EntityStore, StoreConfig, QueryEntity, Paginator } from '@datorama/akita';
import { VehicleDto } from '@app/core/dtos/VehicleDto';
import { createPaginator } from '@app/core/paginator/app-paginator.token';

export interface VehicleState extends EntityState<VehicleDto> { }

@Injectable()
@StoreConfig({ name: 'vehicles' })
export class VehicleStore extends EntityStore<VehicleState, VehicleDto> {
  constructor() {
    super();
  }
}

@Injectable()
export class VehicleQuery extends QueryEntity<VehicleState, VehicleDto> {
  constructor(protected store: VehicleStore) {
    super(store);
  }
}

export const VEHICLE_PAGINATOR = createPaginator<VehicleDto>('VEHICLE_PAGINATOR', VehicleQuery);
