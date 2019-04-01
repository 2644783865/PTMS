import { Injectable } from '@angular/core';
import {
  HttpInterceptor, HttpHandler, HttpRequest
} from '@angular/common/http';
import { AuthTokenService } from '../auth/auth.token.service';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

  constructor(private authTokenService: AuthTokenService) { }

  intercept(req: HttpRequest<any>, next: HttpHandler) {
    const authToken = this.authTokenService.getToken();
    const authReq = req.clone({ setHeaders: { Authorization: authToken } });
    return next.handle(authReq);
  }
}
