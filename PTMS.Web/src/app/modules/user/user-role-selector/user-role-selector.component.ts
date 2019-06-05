import { Component, EventEmitter, Output } from '@angular/core';
import { ControlContainer, FormGroup, Validators } from '@angular/forms';
import { ProjectDto } from '@app/core/dtos/ProjectDto';
import { RoleDto } from '@app/core/dtos/RoleDto';
import { RoleEnum } from '@app/core/enums/role.enum';
import { CustomValidators } from '@app/core/validation';
import { Observable } from 'rxjs';
import { UserQuery } from '../user.state';
import { RouteDto } from '@app/core/dtos/RouteDto';

@Component({
  selector: 'app-user-role-selector',
  templateUrl: 'user-role-selector.component.html',
})
export class UserRoleSelectorComponent {
  @Output() loadProjects = new EventEmitter<any>();
  @Output() loadRoutes = new EventEmitter<number>();

  projects$: Observable<ProjectDto[]>;
  roles$: Observable<RoleDto[]>;
  routes$: Observable<RouteDto[]>;

  roleSelectorForm: FormGroup;
  showProjectsSelect: boolean;
  showRoutesSelect: boolean;

  constructor(
    private userQuery: UserQuery,
    private controlContainer: ControlContainer) {}

  ngOnInit() {
    this.roleSelectorForm = this.controlContainer.control as FormGroup;

    this.roles$ = this.userQuery.roles$;
    this.projects$ = this.userQuery.projects$;
    this.routes$ = this.userQuery.routes$;
  }
  
  ngAfterViewInit() {
    this.roleSelectorForm.get("roleId").setValidators(Validators.required);
    this.roleSelectorForm.get("project").setValidators(CustomValidators.requiredIf('roleId', this.isProjectRequired.bind(this)));
    this.roleSelectorForm.get("routeIds").setValidators(CustomValidators.requiredIf('roleId', this.isRoutesRequired.bind(this)));

    this.roleSelectorForm.get('roleId').valueChanges.subscribe(this.onRoleSelect.bind(this));
    this.roleSelectorForm.get('project').valueChanges.subscribe(this.onProjectSelect.bind(this));
  }

  isProjectRequired(): boolean {
    var roleId = this.roleSelectorForm.get('roleId').value;
    var roleName = this.getSelectedRoleName(roleId);
    return [RoleEnum.Transporter, RoleEnum.Mechanic].includes(roleName);
  }

  isRoutesRequired(): boolean {
    var roleId = this.roleSelectorForm.get('roleId').value;
    return this.getSelectedRoleName(roleId) == RoleEnum.Mechanic
      && this.roleSelectorForm.get('project').value != null;
  }

  onRoleSelect() {
    this.showProjectsSelect = this.isProjectRequired();

    this.roleSelectorForm.get('project').setValue(null);

    if (this.showProjectsSelect && !this.userQuery.getValue().projects) {
      this.loadProjects.emit();
    }
  }

  onProjectSelect() {
    this.showRoutesSelect = this.isRoutesRequired();

    this.roleSelectorForm.get('routeIds').setValue(null);

    if (this.showRoutesSelect) {
      this.loadRoutes.emit(this.roleSelectorForm.get('project').value.id);
    }
  }

  private getSelectedRoleName(roleId: number): RoleEnum {
    let role = this.userQuery.getValue().roles.find(r => r.id == roleId);
    return role && role.name;
  }
}
