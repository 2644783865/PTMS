import { Component, OnInit } from '@angular/core';
import { Validators, FormBuilder, FormGroup } from '@angular/forms';
import { AuthService } from '@app/core/auth/auth.service';
import { Router, ActivatedRoute } from '@angular/router';
import { LoginDto } from '@app/core/dtos/LoginDto';
import { NotificationService } from '@app/core/notification/notification.service';
import { RegisterDto } from '@app/core/dtos/RegisterDto';
import { AccountDataService } from '@app/core/data-services/account.data.service';
import { CustomValidators } from '@app/core/validation';
import { BehaviorSubject } from 'rxjs';

declare function require(name: string);

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  logo: string;
  loginForm: FormGroup;
  registerForm: FormGroup;
  showLoginForm: boolean;
  showRegisterButton: boolean;
  loading$: BehaviorSubject<boolean>;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router,
    private notificationService: NotificationService,
    private route: ActivatedRoute,
    private accountDataService: AccountDataService) { }

  ngOnInit() {
    this.logo = require('../../../assets/logo.png');
    this.showLoginForm = true;
    this.showRegisterButton = true;
    this.loading$ = new BehaviorSubject(false);

    this.loginForm = this.fb.group({
      email: ['', Validators.required],
      password: ['', Validators.required]
    });

    this.registerForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      firstName: ['', [Validators.required, Validators.maxLength(15)]],
      lastName: ['', [Validators.required, Validators.maxLength(20)]],
      middleName: ['', [Validators.required, Validators.maxLength(20)]],
      phoneNumber: ['', [Validators.required, Validators.maxLength(15)]],
      description: ['', [Validators.required, Validators.maxLength(256)]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      comparePassword: ['', [Validators.required, CustomValidators.matchOther('password')]],
    });
  }

  togglePage() {
    this.showLoginForm = !this.showLoginForm;
  }

  onLogin() {
    if (this.loginForm.valid) {
      this.loading$.next(true);

      var dto = this.loginForm.value as LoginDto;

      this.authService.login(dto)
        .subscribe(
          () => {
            let url = this.route.snapshot.queryParams.returnUrl || '/';
            this.router.navigate([url]);
          },
          errorResponse => {
            this.loading$.next(false);
            this.notificationService.error(errorResponse.error.message);
          })
    }
  }

  onRegister() {
    if (this.registerForm.valid) {
      this.loading$.next(true);
      var dto = this.registerForm.value as RegisterDto;

      this.accountDataService.register(dto)
        .subscribe(
          () => {
            this.notificationService.success("Регистрация прошла успешно. Письмо с инструкциями выслано на вашу почту");
            this.showRegisterButton = false;
            this.togglePage();
            this.loading$.next(false);
          },
          errorResponse => {
            this.notificationService.error(errorResponse.error.message);
            this.loading$.next(false);
          });
    }
  }
}
