import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AppPaginationResponse } from '../akita-extensions/app-paged-entity-state';
import { PtmsHttpClient } from '../data-services/ptms.http.client';
import { ObjectDto } from '../dtos/ObjectDto';

@Injectable({
  providedIn: 'root'
})
export class ObjectDataService {

  constructor(private http: PtmsHttpClient) {
  }

  getAll(page: number, pageSize: number, params: object = null): Observable<AppPaginationResponse<ObjectDto>> {
    return this.http.getPage<ObjectDto>('objects', page, pageSize, params);
  }

  getForReporting(minutes: number): Observable<ObjectDto[]> {
    return this.http.get<ObjectDto[]>(`objects/reporting`, { minutes });
  }

  getById(id: number) {
    return this.http.get<ObjectDto>(`object/${id}`);
  }

  update(item: ObjectDto) {
    return this.http.put<ObjectDto>(`object/${item.id}`, item);
  }

  changeRoute(id: number, newRouteId: number) {
    return this.http.post<ObjectDto>(`object/${id}/changeRoute/${newRouteId}`);
  }

  changeProvider(id: number, newProviderId: number) {
    return this.http.post<ObjectDto>(`object/${id}/changeProvider/${newProviderId}`);
  }

  enable(id: number, newRouteId: number) {
    return this.http.post<ObjectDto>(`object/${id}/enable/${newRouteId}`);
  }

  disable(id: number) {
    return this.http.post<ObjectDto>(`object/${id}/disable`);
  }
}
