import { Injectable } from '@angular/core';
import { StoreConfig } from '@datorama/akita';
import { UserDto } from '@app/core/dtos/UserDto';
import { ProjectDto } from '@app/core/dtos/ProjectDto';
import { RoleDto } from '@app/core/dtos/RoleDto';
import { RouteDto } from '@app/core/dtos/RouteDto';
import { AppEntityState, AppEntityStore, AppQueryEntity } from '@app/core/akita-extensions/app-entity-state';

export interface UserUI extends UserDto {
  roleDisplayName: string,
  canChangePassword: boolean;
  canConfirmUser: boolean;
  canToggleUser: boolean;
  statusStyle: string;
  isActive: boolean;
}

export interface UserState extends AppEntityState<UserUI> {
  projects: ProjectDto[];
  roles: RoleDto[];
  routes: RouteDto[];
}

@Injectable()
@StoreConfig({
  name: 'user-page',
  resettable: true
})
export class UserStore extends AppEntityStore<UserState, UserUI> {
  constructor() {
    super();
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

  setRoutes(routes: RouteDto[]) {
    this.update({
      routes
    });
  }
}

@Injectable()
export class UserQuery extends AppQueryEntity<UserState, UserUI> {
  projects$ = this.select(s => s.projects);
  roles$ = this.select(s => s.roles);
  routes$ = this.select(s => s.routes);

  constructor(protected store: UserStore) {
    super(store);
  }
}
