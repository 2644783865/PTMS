import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { SharedModule } from '@app/shared';
import { AuthGuard } from '@app/core/auth';
import { EventLogPageComponent } from './event-log-page.component';
import { EventLogQuery, EventLogStore } from './event-log.state';
import { EventLogService } from './event-log.service';
import { RoleEnum } from '@app/core/enums';

const routes: Routes = [
  {
    path: 'eventLogs',
    component: EventLogPageComponent,
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
    EventLogQuery,
    EventLogStore,
    EventLogService
  ],
  declarations: [
    EventLogPageComponent
  ],
  exports: [
    EventLogPageComponent
  ]
})
export class EventLogsModule { }
