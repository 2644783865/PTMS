import { Injectable } from '@angular/core';
import { HomeStore } from './home.state';
import { ObjectDataService, ProjectDataService, ProviderDataService, RouteDataService, PlanDataService } from '@app/core/data-services';
import { toDate } from '@app/core/helpers';
import { ObjectDto } from '@app/core/dtos';

@Injectable()
export class HomeService {
  constructor(
    private homeStore: HomeStore,
    private objectDataService: ObjectDataService,
    private projectDataService: ProjectDataService,
    private providerDataService: ProviderDataService,
    private routeDataService: RouteDataService,
    private planDataService: PlanDataService)
  {
  }

  async loadRelatedData() {
    this.homeStore.setLoading(true);

    let [ projects, providers, routes, plansByRoutes ] = await Promise.all([
      this.projectDataService.getAll({ active: true }).toPromise(),
      this.providerDataService.getAll().toPromise(),
      this.routeDataService.getAll({ active: true }).toPromise(),
      this.planDataService.getPlansByRoute(toDate(new Date())).toPromise()
    ]);

    this.homeStore.setRelatedData(
      projects,
      providers,
      routes,
      plansByRoutes);

    this.homeStore.setLoading(false);
  }

  async loadObjectsForReporting(intervalId: string) {
    let minutes = this.minutesDictionary[intervalId]();

    this.homeStore.setLoading(true);

    let result = await this.objectDataService.getForReporting(minutes).toPromise();
    this.homeStore.set(result);
    this.homeStore.setLoading(true);

    setTimeout(_ => {
      this.homeStore.setLoading(false);
    }, 1000)
  }

  setRouteStatFilters(filters): void {
    this.homeStore.setRouteStatFilters(filters);
  }
  
  onDestroy() {
    this.homeStore.reset();
  }

  private minutesDictionary = {
    '5': _ => 5,
    '10': _ => 10,
    '30': _ => 30,
    '60': _ => 60,
    '180': _ => 180,
    'today': _ => {
      let startDate = new Date();
      startDate.setHours(4, 59);
      return this.getMinutes(startDate, new Date());
    },
    'week': _ => {
      let d = new Date();
      let day = d.getDay(),
        diff = d.getDate() - day + (day == 0 ? -6 : 1);

      let startDate = new Date(d.setDate(diff)); //monday
      startDate.setHours(4, 59);

      return this.getMinutes(startDate, new Date());
    },
  };

  private getMinutes(startDate: Date, endDate: Date): number {
    let difference = endDate.getTime() - startDate.getTime();
    let minutes = difference / 1000 / 60;

    return minutes;
  }

  private mapToModel(vehicle: ObjectDto): void {
    let state = this.homeStore.getValue();
    vehicle.provider = state.providers.find(x => x.id == vehicle.providerId);
    vehicle.route = state.routes.find(x => x.id == vehicle.lastRout);
    vehicle.project = state.projects.find(x => x.id == vehicle.projId);
  }
}
