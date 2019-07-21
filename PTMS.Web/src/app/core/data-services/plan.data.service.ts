import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PtmsHttpClient } from '../data-services/ptms.http.client';
import { PlanByRouteDto } from '../dtos/PlanByRouteDto';

@Injectable({
  providedIn: 'root'
})
export class PlanDataService {

  constructor(private http: PtmsHttpClient) {
  }

  getPlansByRoute(date: string): Observable<PlanByRouteDto[]> {
    return this.http.get<PlanByRouteDto[]>('plans/byroute', { date: date });
  }
}
