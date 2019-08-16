import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { SharedModule } from '@app/shared';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeModule } from './modules/home/home.module';
import { LayoutModule } from './modules/layout/layout.module';
import { LoginModule } from './modules/login/login.module';
import { RouteModule } from './modules/route/route.module';
import { UserModule } from './modules/user/user.module';
import { ObjectModule } from './modules/object/object.module';
import { ChangeRouteModule } from './modules/change-route/change-route.module';
import { CoreModule } from './core/core.module';

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
    ObjectModule,
    RouteModule,
    UserModule,
    ChangeRouteModule,

    //app
    AppRoutingModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
