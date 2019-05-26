import { Injectable, inject, InjectionToken } from '@angular/core';
import { EntityState, EntityStore, StoreConfig, QueryEntity } from '@datorama/akita';
import { ObjectDto } from '@app/core/dtos/ObjectDto';
import { AppPaginator } from '@app/core/paginator/app-paginator.plugin';

export interface ObjectState extends EntityState<ObjectDto> { }

@Injectable()
@StoreConfig({
  name: 'objects',
  resettable: true
})
export class ObjectStore extends EntityStore<ObjectState, ObjectDto> {
  constructor() {
    super();
  }
}

@Injectable()
export class ObjectQuery extends QueryEntity<ObjectState, ObjectDto> {
  constructor(protected store: ObjectStore) {
    super(store);
  }
}

export const OBJECT_PAGINATOR = new InjectionToken('OBJECT_PAGINATOR', {
  factory: () => {
    return new AppPaginator<ObjectDto>(inject(ObjectQuery));
  }
});
