import { Injectable } from '@angular/core';
import { ObjectDto } from '@app/core/dtos/ObjectDto';
import { EntityState, EntityStore, QueryEntity, StoreConfig } from '@datorama/akita';

export interface ObjectState extends EntityState<ObjectDto> { }

@Injectable()
@StoreConfig({ name: 'change-route', idKey: 'ids' })
export class ChangeRouteStore extends EntityStore<ObjectState, ObjectDto> {
  constructor() {
    super();
  }
}

@Injectable()
export class ChangeRouteQuery extends QueryEntity<ObjectState, ObjectDto> {
  constructor(protected store: ChangeRouteStore) {
    super(store);
  }
}
