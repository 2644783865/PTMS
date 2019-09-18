import { Injectable } from '@angular/core';
import { BusStationDataService } from '@app/core/data-services';
import { applyTransaction } from '@datorama/akita';
import { BusStationDto } from '@app/core/dtos';
import { BusStationStore } from './bus-station.state';
import { ConfirmDialogService } from '@app/shared/confirm-dialog/confirm-dialog.service';
import { NotificationService } from '@app/shared';

@Injectable()
export class BusStationService {
  constructor(
    private busStationStore: BusStationStore,
    private busStationDataService: BusStationDataService,
    private notificationService: NotificationService,
    private confirmDialogService: ConfirmDialogService)
  {
  }  

  async loadData() {
    this.busStationStore.setLoading(true);
    let stations = await this.busStationDataService.getAll();
    
    applyTransaction(() => {
      this.busStationStore.set(stations);
      this.busStationStore.setLoading(false);
    })
  }

  async save(formValue: any): Promise<void> {
    try {
      this.busStationStore.setModalLoading(true);

      let dto = formValue as BusStationDto;

      var station = await this.busStationDataService.addOrUpdate(dto);
      this.busStationStore.addOrUpdate(station.id, station);

      this.notificationService.success(`Остановка ${station.name} была успешно ${dto.id > 0 ? 'отредактирована' : 'добавлена'}`, station.id);
    }
    catch (exc) {
      this.notificationService.exception(exc);
      throw exc;
    }
    finally {
      this.busStationStore.setModalLoading(false);
    }
  }

  async delete(busStation: BusStationDto) {
    let message = `Вы действительно хотите удалить остановку ${busStation.name}?`;
    await this.confirmDialogService.openDialog(message);

    try {
      this.busStationStore.setLoading(true);

      await this.busStationDataService.delete(busStation.id);

      this.busStationStore.remove(busStation.id);

      this.notificationService.success(`Остановка ${busStation.name} был успешна удалена`);
    }
    catch (exc) {
      this.notificationService.exception(exc);
    }
    finally {
      this.busStationStore.setLoading(false);
    }
  }

  onDestroy() {
    this.busStationStore.reset();
  }
}
