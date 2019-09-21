import { Injectable } from '@angular/core';
import { StoreConfig } from '@datorama/akita';
import { EventLogDto, NamedEntityDto } from '@app/core/dtos';
import { AppPagedEntityState, AppPagedEntityStore, AppPagedQueryEntity } from '@app/core/akita-extensions';

export interface EventLogState extends AppPagedEntityState<EventLogDto> { 
  operations: NamedEntityDto[]
}

@Injectable()
@StoreConfig({
  name: 'event-log-page',
  resettable: true
})
export class EventLogStore extends AppPagedEntityStore<EventLogState, EventLogDto> {

  setRelatedData(operations) {
    this.update({
      operations
    })
  }

  constructor() {
    super();
  }
}

@Injectable()
export class EventLogQuery extends AppPagedQueryEntity<EventLogState, EventLogDto> {
  operations$ = this.select(x => x.operations);

  constructor(protected store: EventLogStore) {
    super(store);
  }
}
