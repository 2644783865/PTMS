import { NgModule } from '@angular/core';
import { SharedModule } from '@app/shared';
import { ChangeRouteStore, ChangeRouteQuery } from './change-route.state';
import { ChangeRouteService } from './change-route.service';
import { ChangeRouteComponent } from './change-route.component';
import { ChangeRoutingModule } from './change-route-routing.module';

@NgModule({
  imports: [
    SharedModule,
    ChangeRoutingModule
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
