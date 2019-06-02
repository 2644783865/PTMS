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
  isLoading$: Observable<boolean>;
  list$: Observable<UserUI[]>;
  modalLoading$: Observable<boolean>;
  projects$: Observable<ProjectDto[]>;
  roles$: Observable<RoleDto[]>;
  modalForm: FormGroup;
  showProjectsSelect: boolean;

  constructor(
    private userQuery: UserQuery,
    private userService: UserService,
    private dialogRef: MatDialogRef<UserCreateDialogComponent>,
    private fb: FormBuilder) {}

  ngOnInit() {
    this.modalLoading$ = this.userQuery.modalLoading$;
    this.roles$ = this.userQuery.roles$;
    this.projects$ = this.userQuery.projects$;

    this.modalForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      firstName: ['', [Validators.required, Validators.maxLength(15)]],
      lastName: ['', [Validators.required, Validators.maxLength(20)]],
      middleName: ['', [Validators.required, Validators.maxLength(20)]],
      phoneNumber: ['', [Validators.required, Validators.maxLength(15)]],
      description: ['', [Validators.required, Validators.maxLength(256)]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      comparePassword: ['', [Validators.required, CustomValidators.matchOther('password')]],
      roleId: ['', Validators.required],
      project: ['', CustomValidators.requiredIf('roleId', this.isProjectRequired.bind(this))]
    });

    this.modalForm.get('roleId').valueChanges.subscribe(this.onRoleSelect.bind(this));
  }

  async onConfirm() {
    if (this.modalForm.valid) {
      await this.userService.createUser(this.modalForm.value);
      this.onClose();
    }    
  }

  isProjectRequired(): boolean {
    var roleId = this.modalForm.get('roleId').value;
    let role = this.userQuery.getValue().roles.find(r => r.id == roleId);
    return role && role.name == RoleEnum.Transporter;
  }

  onRoleSelect() {
    this.showProjectsSelect = this.isProjectRequired();

    this.modalForm.get('project').setValue(null);

    if (this.showProjectsSelect) {
      this.userService.loadProjects();
    }
  }

  onClose(): void {
    this.dialogRef.close();
  }
}
