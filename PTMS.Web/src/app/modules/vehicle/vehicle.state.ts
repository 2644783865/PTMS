import { Injectable, inject, InjectionToken } from '@angular/core';
import { EntityState, EntityStore, StoreConfig, QueryEntity } from '@datorama/akita';
import { VehicleDto } from '@app/core/dtos/VehicleDto';
import { AppPaginator } from '@app/core/paginator/app-paginator.plugin';

export interface VehicleState extends EntityState<VehicleDto> { }

@Injectable()
@StoreConfig({
  name: 'vehicles',
  resettable: true
})
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

export const VEHICLE_PAGINATOR = new InjectionToken('VEHICLE_PAGINATOR', {
  factory: () => {
    return new AppPaginator<VehicleDto>(inject(VehicleQuery));
  }
});
