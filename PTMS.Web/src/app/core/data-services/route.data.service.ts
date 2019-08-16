import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PtmsHttpClient } from '../data-services/ptms.http.client';
import { RouteDto, RouteFullDto } from '../dtos';

@Injectable({
  providedIn: 'root'
})
export class RouteDataService {

  constructor(private http: PtmsHttpClient) {
  }

  getAll(params: object = null): Observable<RouteDto[]> {
    return this.http.get<RouteDto[]>(`routes`, params);
  }

  getAllForPage(): Observable<RouteFullDto[]> {
    return this.http.get<RouteFullDto[]>(`routes/forPage`);
  }

  getByIdForEdit(id: number): Observable<RouteFullDto[]> {
    return this.http.get<RouteFullDto[]>(`route/forEdit/${id}`);
  }
}
