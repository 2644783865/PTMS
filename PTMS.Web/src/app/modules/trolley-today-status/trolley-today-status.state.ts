import { Injectable } from '@angular/core';
import { StoreConfig } from '@datorama/akita';
import { TrolleybusTodayStatusDto } from '@app/core/dtos';
import { AppEntityStore, AppEntityState, AppQueryEntity } from '@app/core/akita-extensions';

export interface TrolleybusStatusState extends AppEntityState<TrolleybusTodayStatusDto> { 
}

@Injectable()
@StoreConfig({
  name: 'trolley-today-status-page',
  resettable: true
})
export class TrolleybusStatusStore extends AppEntityStore<TrolleybusStatusState, TrolleybusTodayStatusDto> {
  constructor() {
    super();
  }
}

@Injectable()
export class TrolleybusStatusQuery extends AppQueryEntity<TrolleybusStatusState, TrolleybusTodayStatusDto> {
  constructor(protected store: TrolleybusStatusStore) {
    super(store);
  }
}
