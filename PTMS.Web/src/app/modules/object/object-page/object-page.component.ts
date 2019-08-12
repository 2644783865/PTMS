import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormControl } from '@angular/forms';
import { MatDialog, Sort } from '@angular/material';
import { AppPaginationResponse } from '@app/core/akita-extensions/app-paged-entity-state';
import { CarBrandDto } from '@app/core/dtos/CarBrandDto';
import { CarTypeDto } from '@app/core/dtos/CarTypeDto';
import { ProjectDto } from '@app/core/dtos/ProjectDto';
import { ProviderDto } from '@app/core/dtos/ProviderDto';
import { PaginatorEvent } from '@app/shared/paginator/paginator.event';
import { merge, Observable } from 'rxjs';
import { debounceTime } from 'rxjs/operators';
import { ObjectChangeRouteDialogComponent } from '../object-change-route-dialog/object-change-route-dialog.component';
import { ObjectEnableDialogComponent } from '../object-enable-dialog/object-enable-dialog.component';
import { ObjectService } from '../object.service';
import { ObjectQuery, ObjectUI } from '../object.state';
import { BlockTypeDto } from '@app/core/dtos/BlockTypeDto';
import { ObjectAddEditDialogComponent } from '../object-add-edit-dialog/object-add-edit-dialog.component';

@Component({
  selector: 'app-object-page',
  templateUrl: './object-page.component.html',
  styleUrls: ['./object-page.component.scss']
})
export class ObjectPageComponent implements OnInit {
  private readonly allColumns = ['name', 'route', 'transporter', 'provider', 'lastTime', 'lastStationTime', 'phone', 'block', 'carBrand', 'carType', 'yearRelease', 'status', 'controls'];
  private _updateInterval: number = 1000 * 10; //10 секунд
  private _intervalId;

  pagination$: Observable<AppPaginationResponse<ObjectUI>>;
  dataLoading$: Observable<boolean>;  
  displayedColumns: string[];
  filters: FormGroup;
  statuses: Map<string, number>;
  updatePerInterval: FormControl;

  projects$: Observable<ProjectDto[]>;
  providers$: Observable<ProviderDto[]>;
  carTypes$: Observable<CarTypeDto[]>;
  carBrands$: Observable<CarBrandDto[]>;
  blockTypes$: Observable<BlockTypeDto[]>;

  isTransporter: boolean;
  canAddVehicle: boolean;

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
    this.blockTypes$ = this.objectQuery.blockTypes$;
    
    this.isTransporter = this.objectService.isTransporter;
    this.canAddVehicle = this.objectService.canAddVehicle;

    if (this.isTransporter) {
      let columnsToHide = ['transporter', 'provider', 'phone', 'block'];
      this.displayedColumns = this.allColumns.filter(x => !columnsToHide.includes(x));
    }
    else if (this.objectService.isDispatcher) {
      let columnsToHide = ['phone', 'block'];
      this.displayedColumns = this.allColumns.filter(x => !columnsToHide.includes(x));
    }
    else {
      this.displayedColumns = this.allColumns;
    }

    this.statuses = new Map<string, number>([
      ['Только активные', 1],
      ['Только выведенные', 0],
      ['Все ТС', -1]
    ]);
    
    this.initFilters();
    this.initUpdateInterval();

    await this.objectService.loadRelatedData();

    this.search();
  }

  openChangeRouteDialog(vehicle: ObjectUI) {
    this.dialog.open(ObjectChangeRouteDialogComponent, {
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

  openAddEditDialog(vehicle: ObjectUI) {
    this.dialog.open(ObjectAddEditDialogComponent, {
      width: '600px',
      data: vehicle || null
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
      blockType: [],
      blockNumber: [],
      sortBy: ['lastTime'],
      orderBy: ['desc']
    });
    
    merge(
      this.filters.get('plateNumber').valueChanges,
      this.filters.get('routeName').valueChanges,
      this.filters.get('yearRelease').valueChanges,
      this.filters.get('blockNumber').valueChanges
    )
      .pipe(debounceTime(400))
      .subscribe(_ => this.search());

    merge(
      this.filters.get('project').valueChanges,
      this.filters.get('active').valueChanges,
      this.filters.get('provider').valueChanges,
      this.filters.get('carBrand').valueChanges,
      this.filters.get('carType').valueChanges,
      this.filters.get('blockType').valueChanges
    ).subscribe(_ => this.search());
  }

  private initUpdateInterval() {
    this.updatePerInterval = new FormControl([true]);
    this.updatePerInterval.valueChanges.subscribe(val => {
      if (val) {
        this.reloadCurrentPage();
        this.startUpdateInterval();
      }
      else {
        this.clearInterval();
      }
    });

    this.startUpdateInterval();
  }

  private reloadCurrentPage() {
    let { currentPage, perPage } = this.objectQuery.getValue();
    let pageEvent = {
      page: currentPage,
      pageSize: perPage
    } as PaginatorEvent;

    this.search(pageEvent);
  }

  private startUpdateInterval() {
    let that = this;

    this.clearInterval();

    this._intervalId = setInterval(_ => {
      that.reloadCurrentPage();
    }, this._updateInterval);
  }

  private clearInterval() {
    if (this._intervalId) {
      clearInterval(this._intervalId);
    }
  }
}
