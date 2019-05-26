import { Component, Inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { ProjectDto } from '@app/core/dtos/ProjectDto';
import { RoleDto } from '@app/core/dtos/RoleDto';
import { RoleEnum } from '@app/core/enums/role.enum';
import { CustomValidators } from '@app/core/validation';
import { Observable } from 'rxjs';
import { UserService } from '../user.service';
import { UserQuery, UserUI } from '../user.state';

@Component({
  selector: 'app-user-confirm-dialog',
  templateUrl: 'user-confirm-dialog.component.html',
})
export class UserConfirmDialogComponent {
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
    private dialogRef: MatDialogRef<UserConfirmDialogComponent>,
    private fb: FormBuilder,
    @Inject(MAT_DIALOG_DATA) public user: UserUI) {}

  ngOnInit() {
    this.modalLoading$ = this.userQuery.modalLoading$;
    this.roles$ = this.userQuery.roles$;
    this.projects$ = this.userQuery.projects$;

    this.modalForm = this.fb.group({
      roleId: ['', Validators.required],
      project: ['', CustomValidators.requiredIf('roleId', this.isProjectRequired.bind(this))]
    });

    this.modalForm.get('roleId').valueChanges.subscribe(this.onRoleSelect.bind(this));
  }

  async onConfirm() {
    if (this.modalForm.valid) {
      await this.userService.confirmUser(this.user.id, this.modalForm.value);
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
