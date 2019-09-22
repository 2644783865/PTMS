import { Injectable } from '@angular/core';
import { PtmsHttpClient } from '../data-services/ptms.http.client';
import { AppPaginationResponse } from '../akita-extensions';
import { EventLogDto, NamedEntityDto } from '../dtos';

@Injectable({
  providedIn: 'root'
})
export class EventLogDataService {

  constructor(private http: PtmsHttpClient) {
  }

  getAll(page: number, pageSize: number, params: object = null): Promise<AppPaginationResponse<EventLogDto>> {
    return this.http.getPage<EventLogDto>('eventLogs', page, pageSize, params);
  }

  getOperations(): Promise<NamedEntityDto[]> {
    return this.http.get<NamedEntityDto[]>('eventOperations');
  }
}
