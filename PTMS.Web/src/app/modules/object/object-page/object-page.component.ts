import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialog, Sort } from '@angular/material';
import { AppPaginationResponse } from '@app/core/akita-extensions/app-paged-entity-state';
import { CarBrandDto } from '@app/core/dtos/CarBrandDto';
import { CarTypeDto } from '@app/core/dtos/CarTypeDto';
import { ProjectDto } from '@app/core/dtos/ProjectDto';
import { ProviderDto } from '@app/core/dtos/ProviderDto';
import { PaginatorEvent } from '@app/shared/paginator/paginator.event';
import { merge, Observable } from 'rxjs';
import { debounceTime } from 'rxjs/operators';
import { ObjectChangeProviderDialogComponent } from '../object-change-provider-dialog/object-change-provider-dialog.component';
import { ObjectChangeRouteDialogComponent } from '../object-change-route-dialog/object-change-route-dialog.component';
import { ObjectEnableDialogComponent } from '../object-enable-dialog/object-enable-dialog.component';
import { ObjectService } from '../object.service';
import { ObjectQuery, ObjectUI } from '../object.state';

@Component({
  selector: 'app-object-page',
  templateUrl: './object-page.component.html',
  styleUrls: ['./object-page.component.scss']
})
export class ObjectPageComponent implements OnInit {
  private readonly allColumns = ['name', 'route', 'transporter', 'carBrand', 'carType', 'provider', 'lastTime', 'lastStationTime', 'yearRelease', 'phone', 'status', 'controls'];
  private _updateInterval: number = 1000 * 10; //10 секунд
  private _intervalId;

  pagination$: Observable<AppPaginationResponse<ObjectUI>>;
  dataLoading$: Observable<boolean>;  
  displayedColumns: string[];
  filters: FormGroup;
  statuses: Map<string, number>;

  projects$: Observable<ProjectDto[]>;
  providers$: Observable<ProviderDto[]>;
  carTypes$: Observable<CarTypeDto[]>;
  carBrands$: Observable<CarBrandDto[]>;
  showProjectsSelect: boolean;
  showProviderSelect: boolean;

  constructor(
    private objectQuery: ObjectQuery,
    private objectService: ObjectService,
    private fb: FormBuilder,
    private dialog: MatDialog) { }

  async ngOnInit() {
    this.pagination$ = this.objectQuery.paginationResponse$;
    this.dataLoading$ = this.objectQuery.dataLoading$;

    this.projects$ = this.objectQuery.projects$;
    this.providers$ = this.objectQuery.providers$;
    this.carBrands$ = this.objectQuery.carBrands$;
    this.carTypes$ = this.objectQuery.carTypes$;
    
    let isTransporter = this.objectService.isTransporter;

    if (isTransporter) {
      this.displayedColumns = this.allColumns.filter(x => !['transporter', 'provider', 'phone'].includes(x));
      this.showProjectsSelect = false;
      this.showProviderSelect = false;
    }
    else {
      this.displayedColumns = this.allColumns;
      this.showProjectsSelect = true;
      this.showProviderSelect = true;
    }

    this.statuses = new Map<string, number>([
      ['Только активные', 1],
      ['Только выведенные', 0],
      ['Все ТС', -1]
    ]);

    this.initFilters();

    await this.objectService.loadRelatedData();

    this.search();

    this.startUpdateInterval();
  }

  openChangeRouteDialog(vehicle: ObjectUI) {
    this.dialog.open(ObjectChangeRouteDialogComponent, {
      width: '400px',
      data: vehicle,
      autoFocus: false
    });
  }

  openChangeProviderDialog(vehicle: ObjectUI) {
    this.dialog.open(ObjectChangeProviderDialogComponent, {
      width: '400px',
      data: vehicle,
      autoFocus: false
    });
  }

  openEnableDialog(vehicle: ObjectUI) {
    this.dialog.open(ObjectEnableDialogComponent, {
      width: '400px',
      data: vehicle,
      autoFocus: false
    });
  }

  disableVehicle(vehicle: ObjectUI) {
    this.objectService.disable(vehicle);
  }

  search(event: PaginatorEvent = null) {
    window.setTimeout(() => this.objectService.loadPage(event, this.filters.value));
  }

  trackById(item: ObjectUI) {
    return item.id;
  }

  sortData(event: Sort) {
    this.filters.patchValue({
      sortBy: event.active,
      orderBy: event.direction
    });

    this.search();
  }

  ngOnDestroy() {
    this.clearInterval();
    this.objectService.onDestroy();
  }

  private initFilters() {
    this.filters = this.fb.group({
      plateNumber: [],
      routeName: [],
      project: [],
      active: [-1],
      provider: [],
      carBrand: [],
      carType: [],
      yearRelease: [],
      sortBy: ['lastTime'],
      orderBy: ['desc']
    });
    
    merge(
      this.filters.get('plateNumber').valueChanges,
      this.filters.get('routeName').valueChanges,
      this.filters.get('yearRelease').valueChanges
    )
      .pipe(debounceTime(400))
      .subscribe(_ => this.search());

    merge(
      this.filters.get('project').valueChanges,
      this.filters.get('active').valueChanges,
      this.filters.get('provider').valueChanges,
      this.filters.get('carBrand').valueChanges,
      this.filters.get('carType').valueChanges
    ).subscribe(_ => this.search());
  }

  private startUpdateInterval() {
    let that = this;

    this.clearInterval();

    this._intervalId = setInterval(async _ => {
      let { currentPage, perPage } = that.objectQuery.getValue();
      let pageEvent = {
        page: currentPage,
        pageSize: perPage
      } as PaginatorEvent;

      that.search(pageEvent);
    }, this._updateInterval);
  }

  private clearInterval() {
    if (this._intervalId) {
      clearInterval(this._intervalId);
    }
  }
}
