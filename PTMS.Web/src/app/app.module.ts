import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CoreModule } from '@app/core';
import { SharedModule } from '@app/shared';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeModule } from './modules/home/home.module';
import { LayoutModule } from './modules/layout/layout.module';
import { LoginModule } from './modules/login/login.module';
import { RouteModule } from './modules/route/route.module';
import { UserModule } from './modules/user/user.module';
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
    RouteModule,
    UserModule,

    //app
    AppRoutingModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
