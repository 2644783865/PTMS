import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material';
import { ProjectDto } from '@app/core/dtos/ProjectDto';
import { RoleDto } from '@app/core/dtos/RoleDto';
import { RoleEnum } from '@app/core/enums/role.enum';
import { CustomValidators } from '@app/core/validation';
import { Observable } from 'rxjs';
import { UserService } from '../user.service';
import { UserQuery, UserUI } from '../user.state';

@Component({
  selector: 'app-user-create-dialog',
  templateUrl: 'user-create-dialog.component.html',
})
export class UserCreateDialogComponent {
  modalLoading$: Observable<boolean>;
  modalForm: FormGroup;

  constructor(
    private userQuery: UserQuery,
    private userService: UserService,
    private dialogRef: MatDialogRef<UserCreateDialogComponent>,
    private fb: FormBuilder) {}

  ngOnInit() {
    this.modalLoading$ = this.userQuery.modalLoading$;

    this.modalForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      firstName: ['', [Validators.required, Validators.maxLength(15)]],
      lastName: ['', [Validators.required, Validators.maxLength(20)]],
      middleName: ['', [Validators.required, Validators.maxLength(20)]],
      phoneNumber: ['', [Validators.required, Validators.maxLength(15)]],
      description: ['', [Validators.required, Validators.maxLength(256)]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      comparePassword: ['', [Validators.required, CustomValidators.matchOther('password')]],
      roleId: [''],
      project: [''],
      routeIds: ['']
    });
  }

  async onConfirm() {
    if (this.modalForm.valid) {
      await this.userService.createUser(this.modalForm.value);
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
