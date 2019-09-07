import { Injectable } from '@angular/core';
import { ObjectDataService } from '@app/core/data-services/object.data.service';
import { ObjectDto, RouteDto } from '@app/core/dtos';
import { NotificationService } from '@app/core/notification/notification.service';
import { ChangeRouteStore } from './change-route.state';
import { RouteDataService } from '@app/core/data-services';
import { isNotNullOrEmpty } from '@app/core/helpers';

@Injectable()
export class ChangeRouteService {
  constructor(
    private changeRouteStore: ChangeRouteStore,
    private objectDataService: ObjectDataService,
    private routeDataService: RouteDataService,
    private notificationService: NotificationService) {
  }

  async loadRelatedData() {
    let routes = await this.routeDataService.getAll({active: true});
    this.changeRouteStore.setRelatedData(routes);
  }

  search(plateNumber: string) {
    if (isNotNullOrEmpty(plateNumber)){
      this.objectDataService.getAll(1, 10, { plateNumber, format: 'light', active: true })
      .then((vehicles) => {
        this.changeRouteStore.set(vehicles.data);
      });
    }
    else {
      this.changeRouteStore.set([]);
    }
  }

  async save(vehicle: ObjectDto, newRoute: RouteDto) {
    try {
      this.changeRouteStore.setLoading(true);

      let updateItem = await this.objectDataService.changeRoute(vehicle.id, newRoute.id);
      this.changeRouteStore.set([]);

      this.notificationService.success(`Маршрут автобуса номер ${updateItem.name} был успешно изменён`);

      return updateItem;
    }
    catch (exc) {
      this.notificationService.exception(exc);
      throw exc;
    }
    finally {
      this.changeRouteStore.setLoading(false);
    }
  }

  onDestroy() {
    this.changeRouteStore.destroy();
  }
}
