import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { AuthService } from '@app/core/auth/auth.service';
import { Router } from '@angular/router';
import { RoleEnum } from '@app/core/enums/role.enum';

declare function require(name: string);

@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.scss']
})
export class LayoutComponent implements OnInit {
  logo = require('../../../assets/logo.png');
  navigation = [];
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
          { link: 'routes', label: 'Маршруты', visible: false },
          { link: 'change-route', label: 'Сменить Маршрут ТС', visible: this.authService.isInRole(RoleEnum.Transporter) },
          { link: 'users', label: 'Пользователи', visible: this.authService.isInRole(RoleEnum.Administrator) }
        ];

        this.navigation = routes.filter(x => x.visible);
      }
      else {
        this.navigation = [];
      }
    });
  }

  onLogoutClick() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
