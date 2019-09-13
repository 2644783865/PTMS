import { Injectable } from '@angular/core';
import { StoreConfig, Store, Query } from '@datorama/akita';
import { AccountIdentityDto } from '../dtos/AccountIdentityDto';
import { CookieHelper } from '../helpers';

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

  constructor(private _cookieHelper: CookieHelper) {
    super({
      identity: null as AccountIdentityDto,
      token: null,
      isLogged: false
    });

    let tokenInStorage = _cookieHelper.getCookie("authToken");

    if (tokenInStorage) {
      this.update({ token: tokenInStorage });
    }
  }

  akitaPreUpdate(prevState: AuthState, nextState: AuthState) {
    if (nextState.token) {
      this._cookieHelper.setCookie("authToken", nextState.token, 99);
      nextState.isLogged = true;
    }
    else {
      this._cookieHelper.deleteCookie("authToken");
      nextState.isLogged = false;
    }    

    return nextState;
  }
}

@Injectable()
export class AuthQuery extends Query<AuthState> {
  constructor(protected store: AuthStore) {
    super(store);
  }
}
