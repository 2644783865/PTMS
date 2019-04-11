import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { UserDto } from '@app/core/dtos/UserDto';
import { UserService } from './user.service';
import { AuthService } from '@app/core/auth/auth.service';

@Component({
  selector: 'app-user-page',
  templateUrl: './user-page.component.html'
})
export class UserPageComponent implements OnInit {
  list$: Observable<UserDto[]>;
  dataLoading$: Observable<boolean>;
  displayedColumns: string[];

  constructor(
    private userService: UserService,
    private authService: AuthService) { }

  ngOnInit() {
    this.list$ = this.userService.list$;
    this.dataLoading$ = this.userService.isLoading$;
    this.displayedColumns = ['name', 'role', 'description', 'email', 'phone'];

    this.userService.loadData();
  }

  getRole(role: string): string {
    if (role == 'Administrator') {
      return 'Администратор';
    } else if (role == 'Transporter') {
      return 'Перевозчик';
    } else {
      return 'Диспетчер';
    }
  }

  ngOnDestroy() {
    this.userService.onDestroy();
  }
}
