import { Injectable } from '@angular/core';
import { ObjectDataService } from '@app/core/data-services/object.data.service';
import { ObjectStore } from './object.state';
import { PaginatorEvent } from '@app/shared/paginator/paginator.event';
import { ProjectDataService } from '@app/core/data-services/project.data.service';
import { NotificationService } from '@app/core/notification/notification.service';
import { RouteHelper } from '@app/core/helpers/route.helper';
import { ObjectDto } from '@app/core/dtos/ObjectDto';

@Injectable()
export class ObjectService {
  constructor(
    private objectStore: ObjectStore,
    private objectDataService: ObjectDataService,
    private projectDataService: ProjectDataService,
    private routeHelper: RouteHelper,
    private notificationService: NotificationService)
  {
  }  

  async loadPage(event: PaginatorEvent, searchParams: any) {
    let page = event ? event.page : 1;
    let pageSize = event ? event.pageSize : 10;

    let dto = {
      ...searchParams,
      project: searchParams.project && searchParams.project.id,
      active: this.getActiveFlag(searchParams.active)
    };

    this.objectStore.setLoading(true);
    let response = await this.objectDataService.getAll(page, pageSize, dto).toPromise();

    this.objectStore.setPaginationResponse(response);
    this.objectStore.setLoading(false);
  }

  async loadProjects() {
    let projects = await this.projectDataService.getAll().toPromise();
    this.objectStore.setProjects(projects);
  }

  get routeValidator() {
    return this.routeHelper.validate.bind(this.routeHelper);
  }

  async changeRoute(vehicle: ObjectDto, newRouteName: string) {
    try {
      this.objectStore.setModalLoading(true);

      let newRoute = await this.routeHelper
        .getRouteByName(newRouteName)
        .toPromise()

      if (newRoute) {
        let updateItem = await this.objectDataService.changeRoute(vehicle.ids, newRoute.id).toPromise();

        this.objectStore.update(updateItem.ids, updateItem);

        this.notificationService.success(`Маршрут автобуса номер ${updateItem.name} был успешно изменён`, updateItem.ids);

        return updateItem;
      }
      else {
        this.notificationService.error(`Невозможно изменить маршрут на ${newRouteName}`);
      }
    }
    catch (exc) {
      this.notificationService.exception(exc);
    }
    finally {
      this.objectStore.setModalLoading(false);
    }
  }

  onDestroy() {
    this.objectStore.reset();
  }

  private getActiveFlag(value: number): boolean {
    if (value == 1) {
      return true;
    }
    else if (value == 0) {
      return false;
    }
    else {
      return null;
    }
  }
}
