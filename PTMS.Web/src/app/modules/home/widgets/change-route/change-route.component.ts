import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, Validators } from '@angular/forms';
import { PaginatorEvent } from '@app/shared/paginator/paginator.event';
import { PaginationResponse } from '@datorama/akita';
import { Observable } from 'rxjs';
import { ChangeRouteService, ChangeRouteModel } from './change-route.service';
import { debounceTime } from 'rxjs/operators';
import { __await } from 'tslib';
import { NotificationService } from '@app/core/notification/notification.service';

@Component({
  selector: 'app-change-route-widget',
  templateUrl: './change-route.component.html'
})
export class ChangeRouteComponent implements OnInit {
  readonly displayedColumns = ['plateNumber', 'route', 'controls'];

  pagination$: Observable<PaginationResponse<ChangeRouteModel>>;
  dataLoading$: Observable<boolean>;  
  searchForm = this.formBuilder.group({
    plateNumber: [''],
    routeName: ['']
  });
  
  constructor(
    private changeRouteService: ChangeRouteService,
    private formBuilder: FormBuilder,
    private notificationService: NotificationService
  ) {
  }

  ngOnInit() {
    this.pagination$ = this.changeRouteService.pagination$;
    this.dataLoading$ = this.changeRouteService.isLoading$;

    this.searchForm.valueChanges
      .pipe(debounceTime(300))
      .subscribe(params => {
        this.changeRouteService.changeParams(params);
      });
  }

  onPageParamsChange(event: PaginatorEvent) {
    this.changeRouteService.changePage(event.page, event.pageSize);
  }

  async onSave(item: ChangeRouteModel) {
    let result = await this.changeRouteService.save(item);
    this.notificationService.success(`Маршрут автобуса ${result.plateNumber} был успешно изменён`);
  }

  onCancel(item: ChangeRouteModel) {
    item.newRouteName.setValue(item.vehicle.route.name);
  }

  isValueTheSame(item: ChangeRouteModel) {
    return item.newRouteName.value == item.vehicle.route.name;
  }

  ngOnDestroy() {
    this.changeRouteService.onDestroy();
  }
}
