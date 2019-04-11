import { Injectable, Inject } from '@angular/core';
import { Observable } from 'rxjs';
import { PaginationResponse } from '@datorama/akita';
import { VehicleDto } from '@app/core/dtos/VehicleDto';
import { VehicleDataService } from '@app/core/data-services/vehicle.data.service';
import { VehicleQuery, VEHICLE_PAGINATOR, VehicleStore } from './vehicle.state';
import { AppPaginatorPlugin } from '@app/core/paginator/app-paginator.plugin';

@Injectable()
export class VehicleService {
  public readonly isLoading$: Observable<boolean>;
  public readonly pagination$: Observable<PaginationResponse<VehicleDto>>;

  constructor(
    private vehicleStore: VehicleStore,
    @Inject(VEHICLE_PAGINATOR) private paginatorRef: AppPaginatorPlugin<VehicleDto>,
    private vehicleDataService: VehicleDataService)
  {
    this.paginatorRef.setPageSize(5);

    this.isLoading$ = this.paginatorRef.isLoading$;
    this.pagination$ = this.paginatorRef.getDataObservable((page, pageSize) => {
      return this.vehicleDataService.getAll(page, pageSize);
    });    
  }  

  loadPage(page: number, pageSize: number) {
    this.paginatorRef.setPageParams(page, pageSize);
  }

  onDestroy() {
    this.paginatorRef.destroy({ clearCache: true, currentPage: 1 });
  }
}
