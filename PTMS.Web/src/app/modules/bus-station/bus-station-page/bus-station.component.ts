import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { BusStationDto } from '@app/core/dtos';
import { FormControl } from '@angular/forms';
import { BusStationService } from '../bus-station.service';
import { BusStationQuery } from '../bus-station.state';
import { MatDialog } from '@angular/material';
import { BusStationAddEditDialogComponent } from '../bus-station-add-edit-dialog/bus-station-add-edit-dialog.component';

@Component({
  templateUrl: './bus-station.component.html',
  styleUrls: ['./bus-station-page.component.scss']
})
export class BusStationComponent implements OnInit {
  displayedColumns = ['name', 'lat', 'lon', 'azmth', 'controls'];
  dataLoading$: Observable<boolean>;
  list$: Observable<BusStationDto[]>;
  searchString: FormControl;

  constructor(
    private busStationQuery: BusStationQuery,
    private busStationService: BusStationService,
    private dialog: MatDialog) { }

  async ngOnInit() {
    this.dataLoading$ = this.busStationQuery.dataLoading$;
    this.list$ = this.busStationQuery.list$;

    this.busStationService.loadData();
  }
  
  openAddEditDialog(busStation: BusStationDto) {
    this.dialog.open(BusStationAddEditDialogComponent, {
      width: '600px',
      data: busStation || null
    });
  }

  deleteStation(busStation: BusStationDto){
    this.busStationService.delete(busStation);
  }

  trackByFunc(index: number, busStation: BusStationDto) {
    return busStation.id;
  }

  ngOnDestroy() {
    this.busStationService.onDestroy();
  }
}
