import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from '@app/core/auth/auth.guard';
import { ChangeRouteComponent } from './change-route.component';

const routes: Routes = [
  {
    path: 'change-route',
    component: ChangeRouteComponent,
    canActivate: [AuthGuard]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ChangeRoutingModule { }
