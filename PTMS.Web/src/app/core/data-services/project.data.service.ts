import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PtmsHttpClient } from '../data-services/ptms.http.client';
import { ProjectDto } from '../dtos/ProjectDto';

@Injectable({
  providedIn: 'root'
})
export class ProjectDataService {

  constructor(private http: PtmsHttpClient) {
  }

  getAll(params: object = null): Observable<ProjectDto[]> {
    return this.http.get<ProjectDto[]>('projects', params);
  }

  getByRouteId(routeId: number): Observable<ProjectDto> {
    return this.http.get<ProjectDto>(`project/byroute/${routeId}`);
  }
}
