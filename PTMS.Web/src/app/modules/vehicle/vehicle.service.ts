import { Injectable, Inject } from '@angular/core';
import { Observable } from 'rxjs';
import { delay } from 'rxjs/operators';
import { PaginationResponse } from '@datorama/akita';
import { VehicleDto } from '@app/core/dtos/VehicleDto';
import { VehicleDataService } from '@app/core/data-services/vehicle.data.service';
import { VehicleQuery, VEHICLE_PAGINATOR } from './vehicle.state';
import { AppPaginatorPlugin } from '@app/core/paginator/app-paginator.plugin';

@Injectable()
export class VehicleService {
  public readonly isLoading$: Observable<boolean>;
  public readonly pagination$: Observable<PaginationResponse<VehicleDto>>;

  constructor(
    private vehicleQuery: VehicleQuery,
    @Inject(VEHICLE_PAGINATOR) private paginatorRef: AppPaginatorPlugin<VehicleDto>,
    private vehicleDataService: VehicleDataService)
  {
    this.isLoading$ = this.paginatorRef.isLoading$;
    this.pagination$ = this.paginatorRef.getDataObservable((page, pageSize) => {
      return this.vehicleDataService.getAll(page, pageSize);
    });    
  }  

  loadPage(page: number, pageSize: number) {
    this.paginatorRef.setAll(page, pageSize);
  }

  onDestroy() {
    this.paginatorRef.destroy();
  }
}
