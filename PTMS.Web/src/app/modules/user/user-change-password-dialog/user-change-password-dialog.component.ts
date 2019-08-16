import { Component, Inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { matchOther } from '@app/core/validation';
import { Observable } from 'rxjs';
import { UserService } from '../user.service';
import { UserQuery, UserUI } from '../user.state';

@Component({
  selector: 'app-user-change-password-dialog',
  templateUrl: 'user-change-password-dialog.component.html',
})
export class UserChangePasswordDialogComponent {
  modalLoading$: Observable<boolean>;
  modalForm: FormGroup;

  constructor(
    private userQuery: UserQuery,
    private userService: UserService,
    private dialogRef: MatDialogRef<UserChangePasswordDialogComponent>,
    private fb: FormBuilder,
    @Inject(MAT_DIALOG_DATA) public user: UserUI) {}

  ngOnInit() {
    this.modalLoading$ = this.userQuery.modalLoading$;

    this.modalForm = this.fb.group({
      password: ['', [Validators.required, Validators.minLength(6)]],
      comparePassword: ['', [Validators.required, matchOther('password')]]
    });
  }

  async onSubmit() {
    if (this.modalForm.valid) {
      let newPassword = this.modalForm.get('password').value as string;
      await this.userService.changePassword(this.user, newPassword);
      this.onClose();
    }    
  }

  onClose(): void {
    this.dialogRef.close();
  }
}
