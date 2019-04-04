import { Injectable } from '@angular/core';
import { LoginDto } from '../dtos/LoginDto';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { AuthTokenDto } from '../dtos/AuthTokenDto';
import { AccountIdentityDto } from '../dtos/AccountIdentityDto';
import { AuthStore, AuthState } from './auth.store';
import { PtmsHttpClient } from '../data-services/ptms.http.client';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(
    private http: PtmsHttpClient,
    private authStore: AuthStore) {
  }

  login(dto: LoginDto): Observable<AuthTokenDto> {
    return this.http.post<AuthTokenDto>('account/login', dto)
      .pipe(
        tap(tokenResult => {
          this.authStore.update({ token: tokenResult.token });
        }));
  }

  getIdentity(): Observable<AccountIdentityDto> {
    return this.http.get<AccountIdentityDto>('account/identity')
      .pipe(
        tap(identityResult => {
          this.authStore.update({ identity: identityResult });
        }));
  }

  logout() {
    let state = {
      identity: null as AccountIdentityDto,
      token: null
    } as AuthState;

    this.authStore.update(state);
  }
}
