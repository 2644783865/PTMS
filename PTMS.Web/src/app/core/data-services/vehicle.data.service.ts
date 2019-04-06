import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PtmsHttpClient } from '../data-services/ptms.http.client';
import { VehicleDto } from '../dtos/VehicleDto';
import { PaginationResponse } from '@datorama/akita';

@Injectable({
  providedIn: 'root'
})
export class VehicleDataService {

  constructor(private http: PtmsHttpClient) {
  }

  getAll(page: number, pageSize: number): Observable<PaginationResponse<VehicleDto>> {
    return this.http.getPage<VehicleDto>('vehicles', page, pageSize);
  }
}
