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

  getAll(params: object = null): Promise<RouteDto[]> {
    return this.http.get<RouteDto[]>(`routes`, params);
  }

  getAllForPage(): Promise<RouteFullDto[]> {
    return this.http.get<RouteFullDto[]>(`routes/forPage`);
  }

  getByIdForEdit(id: number): Promise<RouteFullDto[]> {
    return this.http.get<RouteFullDto[]>(`route/forEdit/${id}`);
  }

  add(item: RouteFullDto): Promise<RouteDto> {
    return this.http.post<RouteDto>(`route`, item);
  }

  update(item: RouteFullDto): Promise<RouteDto> {
    return this.http.put<RouteFullDto>(`route/${item.id}`, item);
  }

  addOrUpdate(item: RouteFullDto): Promise<RouteDto> {
    return item.id ? this.update(item) : this.add(item);
  }

  delete(id: number) {
    return this.http.delete(`route/${id}`);
  }
}
