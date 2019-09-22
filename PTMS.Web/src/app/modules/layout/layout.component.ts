import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { AuthService } from '@app/core/auth';
import { Router } from '@angular/router';
import { RoleEnum } from '@app/core/enums';

declare function require(name: string);

@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.scss']
})
export class LayoutComponent implements OnInit {
  logo = require('../../../assets/logo.png');
  allNavigation = [];
  isAuthenticated$: Observable<boolean>;
  isStateLoading$: Observable<boolean>;

  constructor(
    private readonly authService: AuthService,
    private readonly router: Router) { }

  ngOnInit() {
    this.isAuthenticated$ = this.authService.isLoggedIn$;
    this.isStateLoading$ = this.authService.isLoading$;

    this.authService.identity$.subscribe(identity => {
      if (identity) {
        let routes = [
          { link: 'home', label: 'Главная', visible: this.authService.isInRole(RoleEnum.Administrator, RoleEnum.Dispatcher) },
          { link: 'objects', label: 'Транспорт', visible: true },
          { link: 'routes', label: 'Маршруты', visible: this.authService.isInRole(RoleEnum.Administrator, RoleEnum.Dispatcher) },
          { link: 'busStations', label: 'Остановки', visible: this.authService.isInRole(RoleEnum.Administrator, RoleEnum.Dispatcher) },
          { link: 'change-route', label: 'Сменить Маршрут ТС', visible: this.authService.isInRole(RoleEnum.Transporter, RoleEnum.Mechanic) },
          { link: 'users', label: 'Пользователи', visible: this.authService.isInRole(RoleEnum.Administrator) },
          { link: 'eventLogs', label: 'Журнал событий', visible: this.authService.isInRole(RoleEnum.Administrator) }
        ];

        this.allNavigation = routes.filter(x => x.visible);
      }
      else {
        this.allNavigation = [];
      }
    });
  }

  get navigation() {
    let dropdownNavigationItems = this.dictionariesNavigation
      .concat(this.adminNavigation);

    return this.allNavigation.filter(x => {
      return !dropdownNavigationItems.some(d => d.link == x.link);
    });
  }

  get dictionariesNavigation() {
    return this.allNavigation.filter(x => ['routes', 'busStations'].includes(x.link));
  }

  get adminNavigation() {
    return this.allNavigation.filter(x => ['users', 'eventLogs'].includes(x.link));
  }

  onLogoutClick() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
