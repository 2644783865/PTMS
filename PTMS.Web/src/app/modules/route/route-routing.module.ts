import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from '@app/core/auth';
import { RoutePageComponent } from './route-page.component';
import { RouteAddEditComponent } from './route-add-edit/route-add-edit.component';

const routes: Routes = [
  {
    path: 'routes',
    component: RoutePageComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'routes/add',
    component: RouteAddEditComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'routes/:id',
    component: RouteAddEditComponent,
    canActivate: [AuthGuard]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RouteRoutingModule { }
