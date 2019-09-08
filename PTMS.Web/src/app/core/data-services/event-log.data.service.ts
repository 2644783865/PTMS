import { Injectable } from '@angular/core';
import { PtmsHttpClient } from '../data-services/ptms.http.client';
import { AppPaginationResponse } from '../akita-extensions';
import { EventLogDto } from '../dtos/EventLogDto';

@Injectable({
  providedIn: 'root'
})
export class EventLogDataService {

  constructor(private http: PtmsHttpClient) {
  }

  getAll(page: number, pageSize: number, params: object = null): Promise<AppPaginationResponse<EventLogDto>> {
    return this.http.getPage<EventLogDto>('eventLogs', page, pageSize, params);
  }
}
