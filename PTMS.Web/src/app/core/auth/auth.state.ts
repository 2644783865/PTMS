import { Injectable } from '@angular/core';
import { StoreConfig, Store, Query } from '@datorama/akita';
import { AccountIdentityDto } from '../dtos/AccountIdentityDto';

export interface AuthState {
  identity: AccountIdentityDto;
  token: string;
  isLogged: boolean;
}

@Injectable()
@StoreConfig({
  name: 'auth',
  resettable: true
})
export class AuthStore extends Store<AuthState> {

  constructor() {
    super({
      identity: null as AccountIdentityDto,
      token: null,
      isLogged: false
    });

    let tokenInStorage = window.localStorage.getItem("authToken");

    if (tokenInStorage) {
      this.update({ token: tokenInStorage });
    }
  }

  akitaPreUpdate(prevState: AuthState, nextState: AuthState) {
    window.localStorage.setItem("authToken", nextState.token);
    nextState.isLogged = !!nextState.token;

    return nextState;
  }
}

@Injectable()
export class AuthQuery extends Query<AuthState> {
  constructor(protected store: AuthStore) {
    super(store);
  }
}
