import { NgModule } from '@angular/core';
import { SharedModule } from '@app/shared';
import { ChangeRouteStore, ChangeRouteQuery } from './change-route.state';
import { ChangeRouteService } from './change-route.service';
import { ChangeRouteComponent } from './change-route.component';

@NgModule({
  imports: [
    SharedModule
  ],
  providers: [
    ChangeRouteStore,
    ChangeRouteQuery,
    ChangeRouteService
  ],
  declarations: [
    ChangeRouteComponent
  ],
  exports: [
    ChangeRouteComponent
  ]
})
export class ChangeRouteModule { }
