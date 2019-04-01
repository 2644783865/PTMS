import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AuthTokenService } from './auth.token.service';
import { LoginDto } from '../dtos/LoginDto';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { AuthTokenDto } from '../dtos/AuthTokenDto';
import { AccountIdentityDto } from '../dtos/AccountIdentityDto';

@Injectable()
export class AuthService {

  constructor(
    private http: HttpClient,
    private authTokenService: AuthTokenService) {
  }

  login(dto: LoginDto): Observable<AuthTokenDto> {
    return this.http.post<AuthTokenDto>('/account/login', dto)
      .pipe(
        tap(tokenResult => this.authTokenService.setToken(tokenResult.token))
      );
  }

  getIdentity(): Observable<AccountIdentityDto> {
    return this.http.get<AccountIdentityDto>('/account/identity');
  }
}
