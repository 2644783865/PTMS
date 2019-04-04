import { Injectable } from '@angular/core';
import { StoreConfig, Store, ID } from '@datorama/akita';
import { AccountIdentityDto } from '../dtos/AccountIdentityDto';

export interface AuthState {
  identity: AccountIdentityDto;
  token: string;
}

@Injectable({ providedIn: 'root' })
@StoreConfig({
  name: 'auth',
  resettable: true
})
export class AuthStore extends Store<AuthState> {

  constructor() {
    super({
      identity: null as AccountIdentityDto,
      token: null
    });

    let tokenInStorage = window.localStorage.getItem("authToken");

    if (tokenInStorage) {
      this.update({ token: tokenInStorage });
    }
  }

  akitaPreUpdate(prevState: AuthState, nextState: AuthState) {
    window.localStorage.setItem("authToken", nextState.token);
    return nextState;
  }
}
