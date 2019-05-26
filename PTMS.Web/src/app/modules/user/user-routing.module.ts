import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from '@app/core/auth/auth.guard';
import { UserPageComponent } from './user-page/user-page.component';
import { RoleEnum } from '@app/core/enums/role.enum';

const routes: Routes = [
  {
    path: 'users',
    component: UserPageComponent,
    canActivate: [AuthGuard],
    data: {
      roles: [RoleEnum.Administrator]
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class UserRoutingModule { }
