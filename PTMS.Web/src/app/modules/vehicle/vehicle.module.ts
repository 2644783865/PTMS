import { NgModule } from '@angular/core';
import { SharedModule } from '@app/shared';
import { VehiclePageComponent } from './vehicle-page.component';
import { VehicleStore, VehicleQuery } from './vehicle.state';
import { VehicleService } from './vehicle.service';
import { VehicleRoutingModule } from './vehicle-routing.module';

@NgModule({
  imports: [
    SharedModule,
    VehicleRoutingModule
  ],
  providers: [
    VehicleStore,
    VehicleQuery,
    VehicleService
  ],
  declarations: [
    VehiclePageComponent
  ],
  exports: [
    VehiclePageComponent
  ]
})
export class VehicleModule { }
