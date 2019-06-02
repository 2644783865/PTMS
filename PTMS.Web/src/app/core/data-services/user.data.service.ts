import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PtmsHttpClient } from '../data-services/ptms.http.client';
import { UserDto } from '../dtos/UserDto';
import { ConfirmUserDto } from '../dtos/ConfirmUserDto';
import { RoleDto } from '../dtos/RoleDto';
import { ChangePasswordDto } from '../dtos/ChangePasswordDto';
import { NewUserDto } from '../dtos/NewUserDto';

@Injectable({
  providedIn: 'root'
})
export class UserDataService {

  constructor(private http: PtmsHttpClient) {
  }

  getAll(): Observable<UserDto[]> {
    return this.http.get<UserDto[]>(`users`);
  }

  getRoles(): Observable<RoleDto[]> {
    return this.http.get<RoleDto[]>(`roles`);
  }

  create(dto: NewUserDto): Observable<UserDto> {
    return this.http.post<UserDto>(`user`, dto);
  }

  confirmUser(userId: number, dto: ConfirmUserDto): Observable<UserDto> {
    return this.http.post<UserDto>(`user/${userId}/confirm`, dto);
  }

  changePassword(userId: number, dto: ChangePasswordDto): Observable<any> {
    return this.http.post<any>(`user/${userId}/changePassword`, dto);
  }

  toggleUser(userId: number): Observable<UserDto> {
    return this.http.post<UserDto>(`user/${userId}/toggle`);
  }
}
