import { NgModule } from '@angular/core';
import { SharedModule } from '@app/shared';
import { ObjectPageComponent } from './object-page/object-page.component';
import { ObjectStore, ObjectQuery } from './object.state';
import { ObjectService } from './object.service';
import { ObjectRoutingModule } from './object-routing.module';
import { ObjectChangeRouteDialogComponent } from './object-change-route-dialog/object-change-route-dialog.component';
import { ObjectChangeProviderDialogComponent } from './object-change-provider-dialog/object-change-provider-dialog.component';
import { ObjectEnableDialogComponent } from './object-enable-dialog/object-enable-dialog.component';
import { ObjectAddEditDialogComponent } from './object-add-edit-dialog/object-add-edit-dialog.component';

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
    ObjectEnableDialogComponent,
    ObjectAddEditDialogComponent
  ],
  declarations: [
    ObjectPageComponent,
    ObjectChangeRouteDialogComponent,
    ObjectChangeProviderDialogComponent,
    ObjectEnableDialogComponent,
    ObjectAddEditDialogComponent
  ],
  exports: [
    ObjectPageComponent
  ]
})
export class ObjectModule { }
