import { Injectable } from '@angular/core';
import { NotificationService } from '@app/shared';
import { TrolleybusStatusStore } from './trolley-today-status.state';
import { DispatchDataService, ObjectDataService } from '@app/core/data-services';
import { TrolleybusTodayStatusDto } from '@app/core/dtos';

@Injectable()
export class TrolleybusTodayStatusService {
  constructor(
    private trolleybusStatusStore: TrolleybusStatusStore,
    private dispatchDataService: DispatchDataService,
    private objectDataService: ObjectDataService,
    private notificationService: NotificationService)
  {
  }  

  async loadData() {
    this.trolleybusStatusStore.setLoading(true);

    let trolleys = await this.dispatchDataService.getTrolleybusTodayStatus();

    this.trolleybusStatusStore.set(trolleys);
    this.trolleybusStatusStore.setLoading(false);
  }

  async changeRoute(status: TrolleybusTodayStatusDto) {
    try {
      this.trolleybusStatusStore.setLoading(true);

      let updateItem = await this.objectDataService.changeRoute(status.id, status.newRoute.id, true);

      await this.loadData();

      this.notificationService.success(`Маршрут автобуса номер ${updateItem.name} был успешно изменён`, status.id);
      return updateItem;
    }
    catch (exc) {
      this.notificationService.exception(exc);
      throw exc;
    }
    finally {
      this.trolleybusStatusStore.setLoading(false);
    }
  }

  onDestroy() {
    this.trolleybusStatusStore.reset();
  }
}
