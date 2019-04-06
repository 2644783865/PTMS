import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { PaginationResponse } from '@datorama/akita';
import { VehicleDto } from '@app/core/dtos/VehicleDto';
import { VehicleService } from './vehicle.service';
import { PaginatorEvent } from '@app/shared/paginator/paginator.event';

@Component({
  selector: 'app-vehicle-page',
  templateUrl: './vehicle-page.component.html'
})
export class VehiclePageComponent implements OnInit {
  pagination$: Observable<PaginationResponse<VehicleDto>>;
  pageLoading: boolean;
  dataLoading$: Observable<boolean>;
  displayedColumns = ['plateNumber', 'route', 'transporter', 'vehicleType'];

  constructor(private vehicleService: VehicleService) { }

  ngOnInit() {
    this.pagination$ = this.vehicleService.pagination$;
    this.dataLoading$ = this.vehicleService.isLoading$;

    this.pageLoading = true;
    this.dataLoading$.subscribe(x => this.pageLoading = this.pageLoading && x);
  }

  onParamsChange(event: PaginatorEvent) {
    this.vehicleService.loadPage(event.page, event.pageSize);
  }

  ngOnDestroy() {
    this.vehicleService.onDestroy();
  }
}
