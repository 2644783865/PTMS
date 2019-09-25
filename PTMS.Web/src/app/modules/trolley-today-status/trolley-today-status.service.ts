import { Injectable } from '@angular/core';
import { NotificationService } from '@app/shared';
import { TrolleybusStatusStore } from './trolley-today-status.state';
import { DispatchDataService, ObjectDataService } from '@app/core/data-services';

@Injectable()
export class TrolleybusTodayStatusService {
  constructor(
    private trolleybusStatusStore: TrolleybusStatusStore,
    private dispatchDataService: DispatchDataService,
    private objectsDataService: ObjectDataService,
    private notificationService: NotificationService)
  {
  }  

  async loadData() {
    this.trolleybusStatusStore.setLoading(true);

    let trolleys = await this.dispatchDataService.getTrolleybusTodayStatus();

    this.trolleybusStatusStore.set(trolleys);
    this.trolleybusStatusStore.setLoading(false);
  }

  onDestroy() {
    this.trolleybusStatusStore.reset();
  }
}
