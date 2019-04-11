import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { RouteDto } from '@app/core/dtos/RouteDto';
import { RouteDataService } from '@app/core/data-services/route.data.service';
import { RouteQuery, RouteStore } from './route.state';

@Injectable()
export class RouteService {
  public readonly isLoading$: Observable<boolean>;
  public readonly list$: Observable<RouteDto[]>;

  constructor(
    private routeQuery: RouteQuery,
    private routeStore: RouteStore,
    private routeDataService: RouteDataService)
  {
    this.isLoading$ = this.routeQuery.selectLoading();
    this.list$ = this.routeQuery.selectAll();
  }  

  async loadData() {
    let data = await this.routeDataService.getAll().toPromise();
    this.routeStore.set(data);
    this.routeStore.setLoading(false);
  }

  onDestroy() {
    this.routeStore.reset();
  }
}
