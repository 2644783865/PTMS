import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from '@app/core/auth/auth.guard';
import { VehiclePageComponent } from './vehicle-page.component';

const routes: Routes = [
  {
    path: 'vehicles',
    component: VehiclePageComponent,
    canActivate: [AuthGuard]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class VehicleRoutingModule { }
