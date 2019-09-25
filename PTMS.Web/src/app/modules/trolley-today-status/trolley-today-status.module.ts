import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { SharedModule } from '@app/shared';
import { AuthGuard } from '@app/core/auth';
import { RoleEnum } from '@app/core/enums';
import { TrolleybusStatusQuery, TrolleybusStatusStore } from './trolley-today-status.state';
import { TrolleyTodayStatusComponent } from './trolley-today-status.component';
import { TrolleybusTodayStatusService } from './trolley-today-status.service';

const routes: Routes = [
  {
    path: 'trolleyTodayStatus',
    component: TrolleyTodayStatusComponent,
    canActivate: [AuthGuard],
    data: {
      roles: [RoleEnum.Administrator]
    }
  }
];

@NgModule({
  imports: [
    RouterModule.forChild(routes),
    SharedModule,
    RouterModule
  ],
  providers: [
    TrolleybusStatusQuery,
    TrolleybusStatusStore,
    TrolleybusTodayStatusService
  ],
  declarations: [
    TrolleyTodayStatusComponent
  ],
  exports: [
    TrolleyTodayStatusComponent
  ]
})
export class TrolleyTodayStatusModule { }
