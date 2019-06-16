import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PtmsHttpClient } from '../data-services/ptms.http.client';
import { ProviderDto } from '../dtos/ProviderDto';

@Injectable({
  providedIn: 'root'
})
export class ProviderDataService {

  constructor(private http: PtmsHttpClient) {
  }

  getAll(): Observable<ProviderDto[]> {
    return this.http.get<ProviderDto[]>('providers');
  }
}
