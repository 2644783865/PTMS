import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PtmsHttpClient } from '../data-services/ptms.http.client';
import { AuthTokenDto } from '../dtos/AuthTokenDto';
import { LoginDto } from '../dtos/LoginDto';
import { AccountIdentityDto } from '../dtos/AccountIdentityDto';
import { RegisterDto } from '../dtos/RegisterDto';

@Injectable({
  providedIn: 'root'
})
export class AccountDataService {

  constructor(private http: PtmsHttpClient) {
  }

  login(dto: LoginDto): Promise<AuthTokenDto> {
    return this.http.post<AuthTokenDto>('account/login', dto);
  }

  getIdentity(): Promise<AccountIdentityDto> {
    return this.http.get<AccountIdentityDto>('account/identity');
  }

  register(dto: RegisterDto): Promise<object> {
    return this.http.post<object>('account/register', dto);
  }
}
