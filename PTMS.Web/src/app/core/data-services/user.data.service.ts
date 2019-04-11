import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PtmsHttpClient } from '../data-services/ptms.http.client';
import { UserDto } from '../dtos/UserDto';

@Injectable({
  providedIn: 'root'
})
export class UserDataService {

  constructor(private http: PtmsHttpClient) {
  }

  getAll(): Observable<UserDto[]> {
    return this.http.get<UserDto[]>(`users`);
  }
}
