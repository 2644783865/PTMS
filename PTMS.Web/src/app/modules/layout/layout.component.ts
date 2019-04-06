import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { AuthService } from '@app/core/auth/auth.service';
import { Router } from '@angular/router';

declare function require(name: string);

@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.scss']
})
export class LayoutComponent implements OnInit {
  logo = require('../../../assets/logo.png');
  navigation = [
    { link: 'home', label: 'Главная' },
    { link: 'vehicles', label: 'Транспорт' }
  ];
  isAuthenticated$: Observable<boolean>;
  isStateLoading$: Observable<boolean>;

  constructor(
    private readonly authService: AuthService,
    private readonly router: Router) { }

  ngOnInit() {
    this.isAuthenticated$ = this.authService.isLoggedIn$;
    this.isStateLoading$ = this.authService.isLoading$;
  }

  onLogoutClick() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
