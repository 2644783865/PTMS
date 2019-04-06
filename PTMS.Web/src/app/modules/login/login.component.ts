import { Component } from '@angular/core';
import { Validators, FormBuilder } from '@angular/forms';
import { AuthService } from '@app/core/auth/auth.service';
import { Router, ActivatedRoute } from '@angular/router';
import { LoginDto } from '@app/core/dtos/LoginDto';
import { NotificationService } from '@app/core/notification/notification.service';

declare function require(name: string);

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  logo = require('../../../assets/logo.png');
  loginForm = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', Validators.required]
  });

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router,
    private notificationService: NotificationService,
    private route: ActivatedRoute) { }

  onSubmit() {
    if (this.loginForm.valid) {
      var dto = this.loginForm.value as LoginDto;

      this.authService.login(dto)
        .subscribe(
          () => {
            let url = this.route.snapshot.queryParams.returnUrl || '/';
            this.router.navigate([url]);
          },
          errorResponse =>
            this.notificationService.error(errorResponse.error.message));
    }
  }
}
