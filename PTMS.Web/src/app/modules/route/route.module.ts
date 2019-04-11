import { NgModule } from '@angular/core';
import { SharedModule } from '@app/shared';
import { RoutePageComponent } from './route-page.component';
import { RouteStore, RouteQuery } from './route.state';
import { RouteService } from './route.service';
import { RouteRoutingModule } from './route-routing.module';

@NgModule({
  imports: [
    SharedModule,
    RouteRoutingModule
  ],
  providers: [
    RouteStore,
    RouteQuery,
    RouteService
  ],
  declarations: [
    RoutePageComponent
  ],
  exports: [
    RoutePageComponent
  ]
})
export class RouteModule { }
