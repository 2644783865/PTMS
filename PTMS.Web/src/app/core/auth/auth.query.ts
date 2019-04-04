import { Injectable } from '@angular/core';
import { AuthState, AuthStore } from './auth.store';
import { Query } from '@datorama/akita';

@Injectable({
  providedIn: 'root'
})
export class AuthQuery extends Query<AuthState> {
  isLoggedIn$ = this.select(state => !!state.token);
  userIdentity$ = this.select(state => state.identity);
  
  constructor(protected store: AuthStore) {
    super(store);
  }

  getToken(): string {
    return this.getValue().token;
  }
}
