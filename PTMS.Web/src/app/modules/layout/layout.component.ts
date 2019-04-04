import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { AuthQuery } from '@app/core/auth/auth.query';
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
    { link: 'home', label: 'Главная' }
  ];
  isAuthenticated$: Observable<boolean>

  constructor(
    private readonly authQuery: AuthQuery,
    private readonly authService: AuthService,
    private readonly router: Router) { }

  ngOnInit() {
    this.isAuthenticated$ = this.authQuery.isLoggedIn$;
  }

  onLogoutClick() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
