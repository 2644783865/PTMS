import { Injectable } from '@angular/core';
import { PtmsHttpClient } from './ptms.http.client';
import { TrolleybusTodayStatusDto } from '../dtos';

@Injectable({
  providedIn: 'root'
})
export class DispatchDataService {

  constructor(private http: PtmsHttpClient) {
  }

  getTrolleybusTodayStatus(): Promise<TrolleybusTodayStatusDto[]> {
    return this.http.get<TrolleybusTodayStatusDto[]>('trolleybusTodayStatus');
  }
}
