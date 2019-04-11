import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from '@app/core/auth/auth.guard';
import { UserPageComponent } from './user-page.component';
import { Role } from '@app/core/enums/role';

const routes: Routes = [
  {
    path: 'users',
    component: UserPageComponent,
    canActivate: [AuthGuard],
    data: {
      roles: [Role.Administrator]
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class UserRoutingModule { }
