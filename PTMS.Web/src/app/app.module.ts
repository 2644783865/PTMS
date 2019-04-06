import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { CoreModule } from '@app/core';
import { SharedModule } from '@app/shared';

import { LoginModule } from './modules/login/login.module';
import { HomeModule } from './modules/home/home.module';
import { LayoutModule } from './modules/layout/layout.module';
import { VehicleModule } from './modules/vehicle/vehicle.module';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    //angular
    BrowserAnimationsModule,
    BrowserModule,

    //core, shared
    CoreModule,
    SharedModule,

    //components
    LayoutModule,
    LoginModule,
    HomeModule,
    VehicleModule,

    //app
    AppRoutingModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
