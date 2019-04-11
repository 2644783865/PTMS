import { Injectable, Inject } from '@angular/core';
import { Observable } from 'rxjs';
import { PaginationResponse } from '@datorama/akita';
import { VehicleDto } from '@app/core/dtos/VehicleDto';
import { VehicleDataService } from '@app/core/data-services/vehicle.data.service';
import { CHANGE_ROUTE_PAGINATOR, ChangeRouteStore, ChangeRouteQuery } from './change-route.state';
import { AppPaginatorPlugin } from '@app/core/paginator/app-paginator.plugin';
import { RouteHelper } from '@app/core/helpers/route.helper';
import { FormControl, Validators } from '@angular/forms';
import { map } from 'rxjs/operators';

export type ChangeRouteModel = {
  vehicle: VehicleDto,
  newRouteName: FormControl
}

@Injectable()
export class ChangeRouteService {
  public readonly isLoading$: Observable<boolean>;
  public readonly pagination$: Observable<PaginationResponse<ChangeRouteModel>>;

  constructor(
    private changeRouteStore: ChangeRouteStore,
    private changeRouteQuery: ChangeRouteQuery,
    @Inject(CHANGE_ROUTE_PAGINATOR) private paginatorRef: AppPaginatorPlugin<VehicleDto>,
    private vehicleDataService: VehicleDataService,
    private routeHelper: RouteHelper)
  {
    this.paginatorRef.setPageSize(5);

    this.isLoading$ = this.paginatorRef.isLoading$;
    this.pagination$ = this._getPageObservable();
  }  

  changePage(page: number, pageSize: number) {
    this.paginatorRef.setPageParams(page, pageSize);
  }

  changeParams(params: any) {
    this.paginatorRef.setSearchParams(params);
  }

  async save(item: ChangeRouteModel) {
    this.paginatorRef.setLoading(true);

    try {
      let newRoute = await this.routeHelper
        .getRouteByName(item.newRouteName.value as string)
        .toPromise()

      let dto: VehicleDto = {
        ...item.vehicle,
        routeId: newRoute.id
      };

      let updateResult = await this.vehicleDataService.update(dto).toPromise();
      let updateItem = await this.vehicleDataService.getById(updateResult.id).toPromise();
      
      this.changeRouteStore.update(updateItem.id, updateItem);

      return updateItem;
    }
    finally {
      this.paginatorRef.setLoading(false);
    }
  }

  onDestroy() {
    this.paginatorRef.destroy({ clearCache: true, currentPage: 1 });
    this.routeHelper.onDestroy();
  }

  private _getPageObservable() {
    let routeExistValidator = this.routeHelper.validate.bind(this.routeHelper);

    return this.paginatorRef
      .getDataObservable((page, pageSize, searchParams) => {
        return this.vehicleDataService.getAll(page, pageSize, searchParams);
      })
      .pipe(
        map(response => {
          return {
            ...response,
            data: response.data.map(vehicle => {
              return {
                vehicle,
                newRouteName: new FormControl(vehicle.route.name, {
                  validators: [Validators.required],
                  asyncValidators: [routeExistValidator]
                })
              }
            })
          };
        })
      );
  }
}
