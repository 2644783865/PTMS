import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { PaginationResponse } from '@datorama/akita';
import { ObjectDto } from '@app/core/dtos/ObjectDto';
import { ObjectService } from './object.service';
import { PaginatorEvent } from '@app/shared/paginator/paginator.event';
import { AuthService } from '@app/core/auth/auth.service';
import { RoleEnum } from '@app/core/enums/role.enum';

@Component({
  selector: 'app-object-page',
  templateUrl: './object-page.component.html'
})
export class ObjectPageComponent implements OnInit {
  pagination$: Observable<PaginationResponse<ObjectDto>>;
  dataLoading$: Observable<boolean>;
  private readonly allColumns = ['plateNumber', 'route', 'transporter', 'objectType', 'controls'];
  displayedColumns: string[];

  constructor(
    private objectService: ObjectService,
    private authService: AuthService) { }

  ngOnInit() {
    this.pagination$ = this.objectService.pagination$;
    this.dataLoading$ = this.objectService.isLoading$;

    let isTransporter = this.authService.isInRole(RoleEnum.Transporter);

    if (isTransporter) {
      this.displayedColumns = this.allColumns.filter(x => x != 'transporter');
    }
    else {
      this.displayedColumns = this.allColumns;
    }
  }

  onParamsChange(event: PaginatorEvent) {
    this.objectService.loadPage(event.page, event.pageSize);
  }

  ngOnDestroy() {
    this.objectService.onDestroy();
  }
}
