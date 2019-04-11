import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from '@app/core/auth/auth.guard';
import { RoutePageComponent } from './route-page.component';

const routes: Routes = [
  {
    path: 'routes',
    component: RoutePageComponent,
    canActivate: [AuthGuard]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RouteRoutingModule { }
