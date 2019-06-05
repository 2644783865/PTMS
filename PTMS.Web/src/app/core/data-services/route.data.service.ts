import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PtmsHttpClient } from '../data-services/ptms.http.client';
import { RouteDto } from '../dtos/RouteDto';

@Injectable({
  providedIn: 'root'
})
export class RouteDataService {

  constructor(private http: PtmsHttpClient) {
  }

  getAll(params: object = null): Observable<RouteDto[]> {
    return this.http.get<RouteDto[]>(`routes`, params);
  }
}
