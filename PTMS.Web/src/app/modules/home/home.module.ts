import { NgModule } from '@angular/core';
import { SharedModule } from '@app/shared';
import { HomeRoutingModule } from './home-routing.module';
import { HomeComponent } from './home.component';
import { WidgetsModule } from './widgets/widgets.module';

@NgModule({
  imports: [
    SharedModule,
    HomeRoutingModule,
    WidgetsModule
  ],
  declarations: [
    HomeComponent
  ]
})
export class HomeModule { }
