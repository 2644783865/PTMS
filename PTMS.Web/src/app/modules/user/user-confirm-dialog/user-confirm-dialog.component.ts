import { Component, Inject } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { Observable } from 'rxjs';
import { UserService } from '../user.service';
import { UserQuery, UserUI } from '../user.state';

@Component({
  selector: 'app-user-confirm-dialog',
  templateUrl: 'user-confirm-dialog.component.html',
})
export class UserConfirmDialogComponent {
  modalLoading$: Observable<boolean>;
  modalForm: FormGroup;

  constructor(
    private userQuery: UserQuery,
    private userService: UserService,
    private dialogRef: MatDialogRef<UserConfirmDialogComponent>,
    private fb: FormBuilder,
    @Inject(MAT_DIALOG_DATA) public user: UserUI) {}

  ngOnInit() {
    this.modalLoading$ = this.userQuery.modalLoading$;

    this.modalForm = this.fb.group({
      roleId: [''],
      project: [''],
      routeIds: ['']
    });
  }

  async onConfirm() {
    if (this.modalForm.valid) {
      await this.userService.confirmUser(this.user.id, this.modalForm.value);
      this.onClose();
    }    
  }

  loadProjects() {
    this.userService.loadProjects();
  }
  
  loadRoutes(projectId: number) {
    this.userService.loadRoutes(projectId);
  }

  onClose(): void {
    this.dialogRef.close();
  }
}
