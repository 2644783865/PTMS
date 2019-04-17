import { NgModule } from '@angular/core';
import { ChangeRouteModule } from './change-route/change-route.module';

const modules = [
  ChangeRouteModule
];

@NgModule({
  imports: modules,
  exports: modules
})
export class WidgetsModule { }
