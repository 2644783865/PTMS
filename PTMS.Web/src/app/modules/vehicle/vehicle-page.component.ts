import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { PaginationResponse } from '@datorama/akita';
import { VehicleDto } from '@app/core/dtos/VehicleDto';
import { VehicleService } from './vehicle.service';
import { PaginatorEvent } from '@app/shared/paginator/paginator.event';
import { AuthService } from '@app/core/auth/auth.service';
import { Role } from '@app/core/enums/role';

@Component({
  selector: 'app-vehicle-page',
  templateUrl: './vehicle-page.component.html'
})
export class VehiclePageComponent implements OnInit {
  pagination$: Observable<PaginationResponse<VehicleDto>>;
  dataLoading$: Observable<boolean>;
  private readonly allColumns = ['plateNumber', 'route', 'transporter', 'vehicleType', 'controls'];
  displayedColumns: string[];

  constructor(
    private vehicleService: VehicleService,
    private authService: AuthService) { }

  ngOnInit() {
    this.pagination$ = this.vehicleService.pagination$;
    this.dataLoading$ = this.vehicleService.isLoading$;

    let isTransporter = this.authService.isInRole(Role.Transporter);

    if (isTransporter) {
      this.displayedColumns = this.allColumns.filter(x => x != 'transporter');
    }
    else {
      this.displayedColumns = this.allColumns;
    }
  }

  onParamsChange(event: PaginatorEvent) {
    this.vehicleService.loadPage(event.page, event.pageSize);
  }

  ngOnDestroy() {
    this.vehicleService.onDestroy();
  }
}
