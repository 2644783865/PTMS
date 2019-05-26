import { NgModule } from '@angular/core';
import { SharedModule } from '@app/shared';
import { ObjectPageComponent } from './object-page.component';
import { ObjectStore, ObjectQuery } from './object.state';
import { ObjectService } from './object.service';
import { ObjectRoutingModule } from './object-routing.module';

@NgModule({
  imports: [
    SharedModule,
    ObjectRoutingModule
  ],
  providers: [
    ObjectStore,
    ObjectQuery,
    ObjectService
  ],
  declarations: [
    ObjectPageComponent
  ],
  exports: [
    ObjectPageComponent
  ]
})
export class ObjectModule { }
