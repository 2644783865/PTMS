import { NgModule } from '@angular/core';
import { SharedModule } from '@app/shared';
import { UserPageComponent } from './user-page.component';
import { UserStore, UserQuery } from './user.state';
import { UserService } from './user.service';
import { UserRoutingModule } from './user-routing.module';

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
  declarations: [
    UserPageComponent
  ],
  exports: [
    UserPageComponent
  ]
})
export class UserModule { }
