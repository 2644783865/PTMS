import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material';
import { Observable } from 'rxjs';
import { UserService } from '../user.service';
import { UserUI, UserQuery } from '../user.state';
import { UserConfirmDialogComponent } from '../user-confirm-dialog/user-confirm-dialog.component';
import { UserChangePasswordDialogComponent } from '../user-change-password-dialog/user-change-password-dialog.component';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-user-page',
  templateUrl: './user-page.component.html'
})
export class UserPageComponent implements OnInit {
  list$: Observable<UserUI[]>;
  dataLoading$: Observable<boolean>;
  displayedColumns: string[];

  constructor(private userQuery: UserQuery,
    private userService: UserService,
    private dialog: MatDialog,
    private route: ActivatedRoute) { }

  ngOnInit() {
    this.list$ = this.userQuery.selectAll();
    this.dataLoading$ = this.userQuery.selectLoading();
    this.displayedColumns = ['name', 'role', 'email', 'phone', 'description', 'status', 'controls'];

    this.userService.loadData()
      .then(() => {
        let userId = this.route.snapshot.queryParams.id;
        if (userId) {
          let user = this.userQuery.getEntity(userId);

          if (user && user.canConfirmUser) {
            this.confirmAccount(user);
          }
        }
      });
  }

  changePassword(user: UserUI) {
    this.dialog.open(UserChangePasswordDialogComponent, {
      width: '400px',
      data: user
    });
  }

  confirmAccount(user: UserUI) {
    this.dialog.open(UserConfirmDialogComponent, {
      width: '400px',
      data: user
    });
  }

  async toggleAccount(user) {
    await this.userService.toggleAccount(user);
  }

  ngOnDestroy() {
    this.userService.onDestroy();
  }
}
