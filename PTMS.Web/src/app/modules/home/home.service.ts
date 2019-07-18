import { Injectable } from '@angular/core';
import { ObjectDataService } from '@app/core/data-services/object.data.service';
import { ProjectDataService } from '@app/core/data-services/project.data.service';
import { ProviderDataService } from '@app/core/data-services/provider.data.service';
import { RouteDataService } from '@app/core/data-services/route.data.service';
import { ObjectDto } from '@app/core/dtos/ObjectDto';
import { HomeStore } from './home.state';

@Injectable()
export class HomeService {
  constructor(
    private homeStore: HomeStore,
    private objectDataService: ObjectDataService,
    private projectDataService: ProjectDataService,
    private providerDataService: ProviderDataService,
    private routeDataService: RouteDataService)
  {
  }

  async loadRelatedData() {
    this.homeStore.setLoading(true);

    let [ projects, providers, routes ] = await Promise.all([
      this.projectDataService.getAll().toPromise(),
      this.providerDataService.getAll().toPromise(),
      this.routeDataService.getAll().toPromise()
    ]);

    this.homeStore.setRelatedData(
      projects,
      providers,
      routes);

    this.homeStore.setLoading(false);
  }

  async loadObjectsForReporting(minutes: number) {
    this.homeStore.setLoading(true);

    let result = await this.objectDataService.getForReporting(minutes).toPromise();
    this.homeStore.set(result);
    this.homeStore.setLoading(true);

    setTimeout(_ => {
      this.homeStore.setLoading(false);
    }, 1000)
  }
  
  onDestroy() {
    this.homeStore.reset();
  }

  private mapToModel(vehicle: ObjectDto): void {
    let state = this.homeStore.getValue();
    vehicle.provider = state.providers.find(x => x.id == vehicle.providerId);
    vehicle.route = state.routes.find(x => x.id == vehicle.lastRout);
    vehicle.project = state.projects.find(x => x.id == vehicle.projId);
  }
}
