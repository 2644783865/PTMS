import { Injectable } from '@angular/core';
import { EntityState, EntityStore, StoreConfig, QueryEntity } from '@datorama/akita';
import { UserDto } from '@app/core/dtos/UserDto';

export interface UserState extends EntityState<UserDto> { }

@Injectable()
@StoreConfig({
  name: 'user-page',
  resettable: true
})
export class UserStore extends EntityStore<UserState, UserDto> {
  constructor() {
    super();
  }
}

@Injectable()
export class UserQuery extends QueryEntity<UserState, UserDto> {
  constructor(protected store: UserStore) {
    super(store);
  }
}
