import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { AuthService } from './auth.service';
import { AuthState } from './auth.state';
import { RoleEnum } from '../enums/role.enum';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(
    private router: Router,
    private authService: AuthService) { }

  loginPath = 'login';

  getHomePathByRole(state: AuthState): string {
    switch (state.identity.role) {
      case RoleEnum.Transporter:
      case RoleEnum.Mechanic:
        return '/change-route';
      case RoleEnum.Administrator:
        return '/objects';
      default:
        return '/home';
    }
  }

  canActivate(route: ActivatedRouteSnapshot, routerStateSnapshot: RouterStateSnapshot): Observable<boolean> {
    return this.authService.getState()
      .pipe(
        map(state => {
          //if not logged but goes to not login page - redirect to login page
          if (!state.isLogged && route.routeConfig.path !== this.loginPath) {
            this.router.navigate([this.loginPath], { queryParams: { returnUrl: routerStateSnapshot.url } });
            return false;
          }

          //if logged but goes to login page - redirect to home
          if (state.isLogged && route.routeConfig.path === this.loginPath) {
            this.router.navigate([this.getHomePathByRole(state)]);
            return false;
          }

          //check home page
          if (state.isLogged && route.routeConfig.path == 'home' && this.getHomePathByRole(state) != '/') {
            this.router.navigate([this.getHomePathByRole(state)]);
            return false;
          }

          // check if route is restricted by role
          if (route.data.roles) {
            let authorized = route.data.roles.reduce((sum, value) => {
              return sum || state.identity.role == value;
            }, false);

            if (!authorized) {
              this.router.navigate([this.getHomePathByRole(state)]);
              return false;
            }
          }

          // authorized so return true
          return true;
        })
      );
  }
}
