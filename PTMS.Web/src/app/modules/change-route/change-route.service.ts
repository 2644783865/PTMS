import { Injectable } from '@angular/core';
import { ObjectDataService } from '@app/core/data-services/object.data.service';
import { ObjectDto } from '@app/core/dtos/ObjectDto';
import { RouteHelper } from '@app/core/helpers/route.helper';
import { NotificationService } from '@app/core/notification/notification.service';
import { ChangeRouteStore } from './change-route.state';

@Injectable()
export class ChangeRouteService {
  constructor(
    private changeRouteStore: ChangeRouteStore,
    private objectDataService: ObjectDataService,
    private routeHelper: RouteHelper,
    private notificationService: NotificationService) {
  }

  search(plateNumber: string) {
    this.objectDataService.getAll(1, 10, { plateNumber, format: 'light', active: true })
      .subscribe((vehicles) => {
        this.changeRouteStore.set(vehicles.data);
      });
  }

  get routeValidator() {
    return this.routeHelper.validate.bind(this.routeHelper);
  }

  async save(vehicle: ObjectDto, newRouteName: string) {
    try {
      this.changeRouteStore.setLoading(true);

      let newRoute = await this.routeHelper
        .getRouteByName(newRouteName)
        .toPromise()

      if (newRoute) {
        let updateItem = await this.objectDataService.changeRoute(vehicle.id, newRoute.id).toPromise();

        this.changeRouteStore.update(updateItem.id, updateItem);

        this.notificationService.success(`Маршрут автобуса номер ${updateItem.name} был успешно изменён`);

        return updateItem;
      }
      else {
        this.notificationService.error(`Невозможно изменить маршрут на ${newRouteName}`);
      }
    }
    catch (exc) {
      this.notificationService.error(exc.error.message);
    }
    finally {
      this.changeRouteStore.setLoading(false);
    }
  }

  onDestroy() {
    this.routeHelper.onDestroy();
  }
}
