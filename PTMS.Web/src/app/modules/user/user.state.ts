import { Injectable } from '@angular/core';
import { EntityState, EntityStore, StoreConfig, QueryEntity } from '@datorama/akita';
import { UserDto } from '@app/core/dtos/UserDto';
import { ProjectDto } from '@app/core/dtos/ProjectDto';
import { RoleDto } from '@app/core/dtos/RoleDto';

export interface UserUI extends UserDto {
  roleDisplayName: string,
  canChangePassword: boolean;
  canConfirmUser: boolean;
  canToggleUser: boolean;
  statusStyle: string;
  isActive: boolean;
}

export interface UserState extends EntityState<UserUI> {
  modalLoading: boolean;
  projects: ProjectDto[];
  roles: RoleDto[];
}

@Injectable()
@StoreConfig({
  name: 'user-page',
  resettable: true
})
export class UserStore extends EntityStore<UserState, UserUI> {
  constructor() {
    super();
  }

  setModalLoading(modalLoading: boolean) {
    this.update({ modalLoading });
  }

  setProjects(projects: ProjectDto[]) {
    this.update({
      projects
    });
  }

  setRoles(roles: RoleDto[]) {
    this.update({
      roles
    });
  }
}

@Injectable()
export class UserQuery extends QueryEntity<UserState, UserUI> {
  modalLoading$ = this.select(state => state.modalLoading);
  projects$ = this.select(s => s.projects);
  roles$ = this.select(s => s.roles);

  constructor(protected store: UserStore) {
    super(store);
  }
}
