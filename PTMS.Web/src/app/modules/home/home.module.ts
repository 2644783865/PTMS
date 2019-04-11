import { NgModule } from '@angular/core';
import { SharedModule } from '@app/shared';
import { HomeRoutingModule } from './home-routing.module';
import { HomeComponent } from './home.component';
import { ChangeRouteModule } from './widgets/change-route/change-route.module';

@NgModule({
  imports: [
    SharedModule,
    HomeRoutingModule,
    ChangeRouteModule
  ],
  declarations: [
    HomeComponent
  ]
})
export class HomeModule { }
