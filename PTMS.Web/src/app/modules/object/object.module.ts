import { NgModule } from '@angular/core';
import { SharedModule } from '@app/shared';
import { ObjectPageComponent } from './object-page/object-page.component';
import { ObjectStore, ObjectQuery } from './object.state';
import { ObjectService } from './object.service';
import { ObjectRoutingModule } from './object-routing.module';
import { ObjectChangeRouteDialogComponent } from './object-change-route-dialog/object-change-route-dialog.component';
import { ObjectChangeProviderDialogComponent } from './object-change-provider-dialog/object-change-provider-dialog.component';
import { ObjectEnableDialogComponent } from './object-enable-dialog/object-enable-dialog.component';

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
  entryComponents: [
    ObjectChangeRouteDialogComponent,
    ObjectChangeProviderDialogComponent,
    ObjectEnableDialogComponent
  ],
  declarations: [
    ObjectPageComponent,
    ObjectChangeRouteDialogComponent,
    ObjectChangeProviderDialogComponent,
    ObjectEnableDialogComponent
  ],
  exports: [
    ObjectPageComponent
  ]
})
export class ObjectModule { }
