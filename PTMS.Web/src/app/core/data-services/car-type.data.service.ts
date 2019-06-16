import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PtmsHttpClient } from '../data-services/ptms.http.client';
import { CarTypeDto } from '../dtos/CarTypeDto';

@Injectable({
  providedIn: 'root'
})
export class CarTypeDataService {

  constructor(private http: PtmsHttpClient) {
  }

  getAll(): Observable<CarTypeDto[]> {
    return this.http.get<CarTypeDto[]>('carTypes');
  }
}
