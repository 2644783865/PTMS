import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpHandler, HttpRequest } from '@angular/common/http';
import { AuthService } from '../auth/auth.service';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

  constructor(private authService: AuthService) { }

  intercept(req: HttpRequest<any>, next: HttpHandler) {
    const authToken = this.authService.getToken();

    if (authToken) {
      const authReq = req.clone({ setHeaders: { Authorization: 'Bearer ' + authToken } });
      return next.handle(authReq);
    }
    else {
      return next.handle(req);
    }
  }
}
