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

  getAll(): Promise<UserDto[]> {
    return this.http.get<UserDto[]>(`users`);
  }

  getRoles(): Promise<RoleDto[]> {
    return this.http.get<RoleDto[]>(`roles`);
  }

  create(dto: NewUserDto): Promise<UserDto> {
    return this.http.post<UserDto>(`user`, dto);
  }

  confirmUser(userId: number, dto: ConfirmUserDto): Promise<UserDto> {
    return this.http.post<UserDto>(`user/${userId}/confirm`, dto);
  }

  changePassword(userId: number, dto: ChangePasswordDto): Promise<any> {
    return this.http.post<any>(`user/${userId}/changePassword`, dto);
  }

  toggleUser(userId: number): Promise<UserDto> {
    return this.http.post<UserDto>(`user/${userId}/toggle`);
  }
}
