import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AppPaginationResponse } from '../akita-extensions/app-paged-entity-state';
import { PtmsHttpClient } from '../data-services/ptms.http.client';
import { ObjectDto } from '../dtos/ObjectDto';
import { ObjectAddEditRequestDto } from '../dtos/ObjectAddEditRequestDto';

@Injectable({
  providedIn: 'root'
})
export class ObjectDataService {

  constructor(private http: PtmsHttpClient) {
  }

  getAll(page: number, pageSize: number, params: object = null): Promise<AppPaginationResponse<ObjectDto>> {
    return this.http.getPage<ObjectDto>('objects', page, pageSize, params);
  }

  getForReporting(minutes: number): Promise<ObjectDto[]> {
    return this.http.get<ObjectDto[]>(`objects/reporting`, { minutes });
  }

  getById(id: number) {
    return this.http.get<ObjectDto>(`object/${id}`);
  }

  add(item: ObjectAddEditRequestDto) {
    return this.http.post<ObjectDto>(`object`, item);
  }

  update(id: number, item: ObjectAddEditRequestDto) {
    return this.http.put<ObjectDto>(`object/${id}`, item);
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
