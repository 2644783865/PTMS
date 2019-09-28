import { Injectable } from '@angular/core';
import { HomeStore } from './home.state';
import { ObjectDataService, ProjectDataService, ProviderDataService, RouteDataService, PlanDataService } from '@app/core/data-services';
import { toDate, toDateTime } from '@app/core/helpers';
import { EventLogDataService } from '@app/core/data-services/event-log.data.service';

@Injectable()
export class HomeService {
  constructor(
    private homeStore: HomeStore,
    private objectDataService: ObjectDataService,
    private projectDataService: ProjectDataService,
    private providerDataService: ProviderDataService,
    private routeDataService: RouteDataService,
    private planDataService: PlanDataService,
    private eventLogDataService: EventLogDataService)
  {
  }

  async loadRelatedData() {
    this.homeStore.setLoading(true);

    let [ projects, providers, routes, plansByRoutes ] = await Promise.all([
      this.projectDataService.getAll({ active: true }),
      this.providerDataService.getAll(),
      this.routeDataService.getAll({ active: true }),
      this.planDataService.getPlansByRoute(toDate(new Date()))
    ]);

    this.homeStore.setRelatedData(
      projects,
      providers,
      routes,
      plansByRoutes);

    this.homeStore.setLoading(false);
  }

  async loadObjectsForReporting(intervalId: string): Promise<boolean> {
    try {
      let minutes = this.minutesDictionary[intervalId]();

      this.homeStore.setLoading(true);
  
      let result = await this.objectDataService.getForReporting(minutes);
      this.homeStore.set(result);
      this.homeStore.setLoading(true);
  
      setTimeout(_ => {
        this.homeStore.setLoading(false);
      }, 1000)

      return true;
    }
    catch (err) {
      console.error(err);

      return false;
    }
  }

  async loadEventLogs(): Promise<void> {
    try {
      let startDate = new Date();
      startDate.setHours(5, 0, 0, 0);

      let params = {
        eventEnum: 4, //"ChangeObjectRoute",
        startDate: toDateTime(startDate),
        onlyProject: true
      }

      let result = await this.eventLogDataService.getAll(1, 25, params);
      this.homeStore.setEventLogs(result.data);
    }
    catch (err) {
      console.error(err);
    }
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

    return Math.ceil(minutes);
  }
}
