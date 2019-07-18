import { NgModule } from '@angular/core';
import { SharedModule } from '@app/shared';
import { HomeRoutingModule } from './home-routing.module';
import { HomeComponent } from './home.component';
import { HomeStore, HomeQuery } from './home.state';
import { HomeService } from './home.service';

@NgModule({
  imports: [
    SharedModule,
    HomeRoutingModule
  ],
  providers: [
    HomeStore,
    HomeQuery,
    HomeService
  ],
  declarations: [
    HomeComponent
  ]
})
export class HomeModule { }
