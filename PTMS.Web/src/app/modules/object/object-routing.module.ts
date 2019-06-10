import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from '@app/core/auth/auth.guard';
import { ObjectPageComponent } from './object-page/object-page.component';

const routes: Routes = [
  {
    path: 'objects',
    component: ObjectPageComponent,
    canActivate: [AuthGuard]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ObjectRoutingModule { }
