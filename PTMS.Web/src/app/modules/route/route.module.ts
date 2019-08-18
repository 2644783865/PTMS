import { NgModule } from '@angular/core';
import { SharedModule } from '@app/shared';
import { RoutePageComponent } from './route-page.component';
import { RouteStore, RouteQuery } from './route.state';
import { RouteService } from './route.service';
import { RouteRoutingModule } from './route-routing.module';
import { RouteAddEditComponent } from './route-add-edit/route-add-edit.component';

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
    RoutePageComponent,
    RouteAddEditComponent
  ],
  exports: [
    RoutePageComponent
  ]
})
export class RouteModule { }
