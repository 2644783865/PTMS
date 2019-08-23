import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PtmsHttpClient } from '../data-services/ptms.http.client';
import { BusStationDto } from '../dtos';

@Injectable({
  providedIn: 'root'
})
export class BusStationDataService {

  constructor(private http: PtmsHttpClient) {
  }

  getAll(): Promise<BusStationDto[]> {
    return this.http.get<BusStationDto[]>('busStations');
  }

  add(item: BusStationDto): Promise<BusStationDto> {
    return this.http.post<BusStationDto>(`busStation`, item);
  }

  update(item: BusStationDto): Promise<BusStationDto> {
    return this.http.put<BusStationDto>(`busStation/${item.id}`, item);
  }

  addOrUpdate(item: BusStationDto): Promise<BusStationDto> {
    return item.id ? this.update(item) : this.add(item);
  }

  delete(id: number): Promise<Object> {
    return this.http.delete(`busStation/${id}`);
  }
}
