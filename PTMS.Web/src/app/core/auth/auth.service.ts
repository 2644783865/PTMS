import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import { AccountDataService } from '../data-services/account.data.service';
import { AccountIdentityDto } from '../dtos/AccountIdentityDto';
import { AuthTokenDto } from '../dtos/AuthTokenDto';
import { LoginDto } from '../dtos/LoginDto';
import { RoleEnum } from '../enums/role.enum';
import { AuthQuery, AuthState, AuthStore } from './auth.state';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private isAuthInProcess$: BehaviorSubject<boolean>;
  isLoggedIn$ = this.authQuery.select(state => state.isLogged);

  constructor(
    private accountDataService: AccountDataService,
    private authStore: AuthStore,
    private authQuery: AuthQuery) {

    this.isAuthInProcess$ = new BehaviorSubject(false);
  }
  
  getToken(): string {
    return this.authQuery.getValue().token;
  }

  get userId(): number {
    let identity = this.authQuery.getValue().identity;
    return identity ? identity.id : null;
  }

  get isLoading$() {
    return this.isAuthInProcess$.asObservable();
  }

  get identity$() {
    return this.authQuery.select(s => s.identity);
  }

  login(dto: LoginDto): Observable<AuthTokenDto> {
    return this.accountDataService.login(dto)
      .pipe(
        tap(tokenResult => {
          this.authStore.update({ token: tokenResult.token });
        }));
  }

  getState(): Observable<AuthState> {
    let state = this.authQuery.getValue();
    let isAuthInProcess$ = this.isAuthInProcess$;

    //If user is not logged or is logged and roles are loaded
    if (!state.token || state.identity) {
      return of(state);
    }

    let timeoutId = setTimeout(function () {
      isAuthInProcess$.next(true);
    }, 300)

    return this.accountDataService.getIdentity()
      .pipe(
        tap(_ => {
          clearTimeout(timeoutId);
          isAuthInProcess$.next(false);
        }),
        map(identityResult => {
          this.authStore.update({ identity: identityResult });
          return this.authQuery.getValue();
        }),
        catchError(err => {
          this.logout();

          clearTimeout(timeoutId);
          isAuthInProcess$.next(false);

          return of(this.authQuery.getValue());
        })
      );
  }

  logout() {
    let state = {
      identity: null as AccountIdentityDto,
      token: ''
    } as AuthState;

    this.authStore.update(state);
  }

  isInRole(...roles: RoleEnum[]): boolean {
    let identity = this.authQuery.getValue().identity;

    if (identity) {
      return roles.reduce((sum, value) => {
        return sum || identity.role == value;
      }, false);
    }
    else {
      throw new Error('User identity is not loaded');
    }
  }
}
