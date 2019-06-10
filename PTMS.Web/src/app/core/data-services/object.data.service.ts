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

  getById(id: number) {
    return this.http.get<ObjectDto>(`object/${id}`);
  }

  update(item: ObjectDto) {
    return this.http.put<ObjectDto>(`object/${item.ids}`, item);
  }

  changeRoute(id: number, newRouteId: number) {
    return this.http.post<ObjectDto>(`object/${id}/changeRoute/${newRouteId}`);
  }
}
