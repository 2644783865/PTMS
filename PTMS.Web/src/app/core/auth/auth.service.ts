import { Injectable } from '@angular/core';
import { LoginDto } from '../dtos/LoginDto';
import { Observable, of, BehaviorSubject } from 'rxjs';
import { tap, map, catchError } from 'rxjs/operators';
import { AuthTokenDto } from '../dtos/AuthTokenDto';
import { AccountIdentityDto } from '../dtos/AccountIdentityDto';
import { AuthStore, AuthState, AuthQuery } from './auth.state';
import { PtmsHttpClient } from '../data-services/ptms.http.client';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private isAuthInProcess$: BehaviorSubject<boolean>;
  isLoggedIn$ = this.authQuery.select(state => state.isLogged);

  constructor(
    private http: PtmsHttpClient,
    private authStore: AuthStore,
    private authQuery: AuthQuery) {

    this.isAuthInProcess$ = new BehaviorSubject(false);
  }
  
  getToken(): string {
    return this.authQuery.getValue().token;
  }

  get isLoading$() {
    return this.isAuthInProcess$.asObservable();
  }

  login(dto: LoginDto): Observable<AuthTokenDto> {
    return this.http.post<AuthTokenDto>('account/login', dto)
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

    return this.http.get<AccountIdentityDto>('account/identity')
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
      token: null
    } as AuthState;

    this.authStore.update(state);
  }
}
