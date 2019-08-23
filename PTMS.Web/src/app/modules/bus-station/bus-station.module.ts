import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { SharedModule } from '@app/shared';
import { BusStationComponent } from './bus-station-page/bus-station.component';
import { AuthGuard } from '@app/core/auth';
import { BusStationQuery, BusStationStore } from './bus-station.state';
import { BusStationService } from './bus-station.service';
import { BusStationAddEditDialogComponent } from './bus-station-add-edit-dialog/bus-station-add-edit-dialog.component';

const routes: Routes = [
  {
    path: 'busStations',
    component: BusStationComponent,
    canActivate: [AuthGuard]
  }
];

@NgModule({
  imports: [
    RouterModule.forChild(routes),
    SharedModule,
    RouterModule
  ],
  providers: [
    BusStationQuery,
    BusStationStore,
    BusStationService
  ],
  entryComponents: [
    BusStationAddEditDialogComponent
  ],
  declarations: [
    BusStationAddEditDialogComponent,
    BusStationComponent
  ],
  exports: [
    BusStationComponent
  ]
})
export class BusStationModule { }
