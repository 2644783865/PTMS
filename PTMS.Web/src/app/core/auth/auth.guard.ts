import { Router, CanActivate } from '@angular/router';
import { Injectable } from '@angular/core';
import { map, take } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { AuthQuery } from './auth.query';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private router: Router, private authQuery: AuthQuery) {}

  canActivate(): Observable<boolean> {
    return this.authQuery.isLoggedIn$.pipe(
      map(isAuth => {
        if (isAuth) {
          return true;
        }
        this.router.navigateByUrl('login');
        return false;
      }),
      take(1)
    );
  }
}
