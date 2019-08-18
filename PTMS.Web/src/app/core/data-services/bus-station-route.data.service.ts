import { Injectable } from '@angular/core';
import { PtmsHttpClient } from '../data-services/ptms.http.client';
import { BusStationRouteDto } from '../dtos';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class BusStationRouteDataService {

  constructor(private http: PtmsHttpClient) {
  }

  add(item: BusStationRouteDto): Promise<BusStationRouteDto> {
    return this.http.post<BusStationRouteDto>(`busStationRoute`, item);
  }

  update(item: BusStationRouteDto): Promise<BusStationRouteDto> {
    return this.http.put<BusStationRouteDto>(`busStationRoute/${item.id}`, item);
  }

  addOrUpdate(item: BusStationRouteDto): Promise<BusStationRouteDto> {
    return item.id ? this.update(item) : this.add(item);
  }

  delete(id: number) {
    return this.http.delete(`busStationRoute/${id}`);
  }
}
