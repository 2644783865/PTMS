import { Injectable } from '@angular/core';
import { RouteStore } from './route.state';
import { RouteDataService, ProjectDataService } from '@app/core/data-services';

@Injectable()
export class RouteService {
  constructor(
    private routeStore: RouteStore,
    private routeDataService: RouteDataService,
    private projectDataService: ProjectDataService)
  {
  }  

  async loadData() {
    this.routeStore.setLoading(true);
    let projects = await this.projectDataService.getAll().toPromise();
    let routes = await this.routeDataService.getAllForPage().toPromise();
    
    routes.forEach(route => {
      if (route.projectId) {
        route.project = projects.find(p => p.id == route.projectId);
      }
    })

    this.routeStore.set(routes);
    this.routeStore.setLoading(false);
  }

  onDestroy() {
    this.routeStore.reset();
  }
}
