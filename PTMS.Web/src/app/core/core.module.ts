import { NgModule, Optional, SkipSelf } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { httpInterceptorProviders } from './http-interceptors';
import { AuthStore, AuthQuery } from './auth/auth.state';


@NgModule({
  imports: [
    CommonModule,
    HttpClientModule
  ],
  declarations: [],
  providers: [
    httpInterceptorProviders,
    AuthStore,
    AuthQuery
  ]
})
export class CoreModule {
  constructor(
    @Optional()
    @SkipSelf()
    parentModule: CoreModule
  ) {
    if (parentModule) {
      throw new Error('CoreModule is already loaded. Import only in AppModule');
    }
  }
}
