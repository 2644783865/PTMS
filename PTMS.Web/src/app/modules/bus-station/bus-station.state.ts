import { Injectable } from '@angular/core';
import { StoreConfig } from '@datorama/akita';
import { BusStationDto } from '@app/core/dtos';
import { AppEntityStore, AppEntityState, AppQueryEntity } from '@app/core/akita-extensions';

export interface BusStationState extends AppEntityState<BusStationDto> { }

@Injectable()
@StoreConfig({
  name: 'bus-station-page',
  resettable: true
})
export class BusStationStore extends AppEntityStore<BusStationState, BusStationDto> {
  constructor() {
    super();
  }
}

@Injectable()
export class BusStationQuery extends AppQueryEntity<BusStationState, BusStationDto> {
  constructor(protected store: BusStationStore) {
    super(store);
  }
}
