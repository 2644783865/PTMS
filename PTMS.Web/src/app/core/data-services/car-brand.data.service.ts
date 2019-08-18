import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PtmsHttpClient } from '../data-services/ptms.http.client';
import { CarBrandDto } from '../dtos/CarBrandDto';

@Injectable({
  providedIn: 'root'
})
export class CarBrandDataService {

  constructor(private http: PtmsHttpClient) {
  }

  getAll(): Promise<CarBrandDto[]> {
    return this.http.get<CarBrandDto[]>('carBrands');
  }
}
