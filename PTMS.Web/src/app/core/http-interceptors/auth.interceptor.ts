import { Injectable } from '@angular/core';
import {
  HttpInterceptor, HttpHandler, HttpRequest
} from '@angular/common/http';
import { AuthQuery } from '../auth/auth.query';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

  constructor(private authQuery: AuthQuery) { }

  intercept(req: HttpRequest<any>, next: HttpHandler) {
    const authToken = this.authQuery.getToken();

    if (authToken) {
      const authReq = req.clone({ setHeaders: { Authorization: authToken } });
      return next.handle(authReq);
    }
    else {
      return next.handle(req);
    }
  }
}
