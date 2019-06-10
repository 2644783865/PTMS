import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { ObjectDto } from '@app/core/dtos/ObjectDto';
import { ObjectService } from '../object.service';
import { PaginatorEvent } from '@app/shared/paginator/paginator.event';
import { AuthService } from '@app/core/auth/auth.service';
import { RoleEnum } from '@app/core/enums/role.enum';
import { FormGroup, FormBuilder } from '@angular/forms';
import { ObjectQuery } from '../object.state';
import { debounceTime } from 'rxjs/operators';
import { ProjectDto } from '@app/core/dtos/ProjectDto';
import { MatDialog } from '@angular/material';
import { ObjectChangeRouteDialogComponent } from '../object-change-route-dialog/object-change-route-dialog.component';
import { AppPaginationResponse } from '@app/core/akita-extensions/app-paged-entity-state';

@Component({
  selector: 'app-object-page',
  templateUrl: './object-page.component.html'
})
export class ObjectPageComponent implements OnInit {
  private readonly allColumns = ['plateNumber', 'route', 'transporter', 'carBrand', 'carType', 'yearRelease', 'lastTime', 'status', 'controls'];

  pagination$: Observable<AppPaginationResponse<ObjectDto>>;
  dataLoading$: Observable<boolean>;  
  displayedColumns: string[];
  filters: FormGroup;
  statuses: Map<string, number>;

  projects$: Observable<ProjectDto[]>;
  showProjectsSelect: boolean;

  constructor(
    private objectQuery: ObjectQuery,
    private objectService: ObjectService,
    private authService: AuthService,
    private fb: FormBuilder,
    private dialog: MatDialog) { }

  ngOnInit() {
    this.pagination$ = this.objectQuery.paginationResponse$;
    this.dataLoading$ = this.objectQuery.select(x => x.loading);
    this.projects$ = this.objectQuery.projects$;

    let isTransporter = this.authService.isInRole(RoleEnum.Transporter)
      || this.authService.isInRole(RoleEnum.Mechanic);

    if (isTransporter) {
      this.displayedColumns = this.allColumns.filter(x => x != 'transporter');
      this.showProjectsSelect = false;
    }
    else {
      this.displayedColumns = this.allColumns;
      this.showProjectsSelect = true;
      this.objectService.loadProjects()
    }

    this.statuses = new Map<string, number>([
      ['Только активные', 1],
      ['Только выведенные', 0],
      ['Все ТС', -1]
    ]);

    this.initFilters();

    this.search();
  }

  openChangeRouteDialog(vehicle: ObjectDto) {
    this.dialog.open(ObjectChangeRouteDialogComponent, {
      width: '400px',
      data: vehicle
    });
  }

  search(event: PaginatorEvent = null) {
    window.setTimeout(() => this.objectService.loadPage(event, this.filters.value));
  }

  ngOnDestroy() {
    this.objectService.onDestroy();
  }

  private initFilters() {
    this.filters = this.fb.group({
      plateNumber: [],
      routeName: [],
      project: [],
      active: [1]
    });

    this.filters.get('plateNumber')
      .valueChanges
      .pipe(debounceTime(400))
      .subscribe(_ => this.search());

    this.filters.get('routeName')
      .valueChanges
      .pipe(debounceTime(400))
      .subscribe(_ => this.search());

    this.filters.get('project')
      .valueChanges
      .subscribe(_ => this.search());

    this.filters.get('active')
      .valueChanges
      .subscribe(_ => this.search());
  }
}
