import { NgModule } from '@angular/core';
import { SharedModule } from '@app/shared';
import { UserPageComponent } from './user-page/user-page.component';
import { UserStore, UserQuery } from './user.state';
import { UserService } from './user.service';
import { UserRoutingModule } from './user-routing.module';
import { UserConfirmDialogComponent } from './user-confirm-dialog/user-confirm-dialog.component';
import { UserChangePasswordDialogComponent } from './user-change-password-dialog/user-change-password-dialog.component';

@NgModule({
  imports: [
    SharedModule,
    UserRoutingModule
  ],
  providers: [
    UserStore,
    UserQuery,
    UserService
  ],
  entryComponents: [
    UserConfirmDialogComponent,
    UserChangePasswordDialogComponent
  ],
  declarations: [
    UserPageComponent,
    UserConfirmDialogComponent,
    UserChangePasswordDialogComponent
  ],
  exports: [
    UserPageComponent
  ]
})
export class UserModule { }
