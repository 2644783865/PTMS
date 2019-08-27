import { Injectable } from '@angular/core';
import { UserStore, UserUI } from './user.state';
import { UserDataService, ProjectDataService, RouteDataService } from '@app/core/data-services';
import { NotificationService } from '@app/core/notification';
import { AuthService } from '@app/core/auth';
import { NewUserDto, ConfirmUserDto, ChangePasswordDto, UserDto } from '@app/core/dtos';
import { UserStatusEnum } from '@app/core/enums';

@Injectable()
export class UserService {

  constructor(
    private userStore: UserStore,
    private userDataService: UserDataService,
    private projectDataService: ProjectDataService,
    private notificationService: NotificationService,
    private authService: AuthService,
    private routeDataService: RouteDataService)
  {
  }  

  async loadData() {
    let [roles, data] = await Promise.all([
      this.userDataService.getRoles(),
      this.userDataService.getAll()
    ]);

    let loggedUserId = this.authService.userId;
    let result = data.map(user => this.mapToModel(user, loggedUserId));

    this.userStore.set(result);
    this.userStore.setRoles(roles);
    this.userStore.setLoading(false);
  }

  async createUser(formData: any) {
    try {
      this.userStore.setModalLoading(true);

      let dto = {
        ...formData,
        projectId: formData.project ? formData.project.id : null
      } as NewUserDto;

      let userDto = await this.userDataService.create(dto);
      let userUi = this.mapToModel(userDto, this.authService.userId);

      this.userStore.add(userUi);
      this.notificationService.success(`Пользователь '${userUi.fullName}' успешно создан.`, userUi.id);
    }
    catch (exc) {
      this.notificationService.exception(exc);
    }
    finally {
      this.userStore.setModalLoading(false);
    }
  }

  async confirmUser(userId: number, formData: any) {
    try {
      this.userStore.setModalLoading(true);

      let dto = {
        ...formData,
        projectId: formData.project ? formData.project.id : null
      } as ConfirmUserDto;

      let userDto = await this.userDataService.confirmUser(userId, dto);
      let userUi = this.mapToModel(userDto, this.authService.userId);

      this.userStore.update(userUi.id, userUi);
      this.notificationService.success(`Пользователь '${userUi.fullName}' успешно подтверждён.`, userUi.id);
    }
    catch (exc) {
      this.notificationService.exception(exc);
    }
    finally {
      this.userStore.setModalLoading(false);
    }
  }

  async toggleAccount(user: UserUI) {
    try {
      this.userStore.setLoading(true);
      let userDto = await this.userDataService.toggleUser(user.id);
      let userUi = this.mapToModel(userDto, this.authService.userId);

      this.userStore.update(userUi.id, userUi);

      this.notificationService.success(`Пользователь '${userUi.fullName}' успешно ${user.isActive ? 'заблокирован' : 'разблокирован'}.`, userUi.id);
    }
    catch (exc) {
      this.notificationService.exception(exc);
    }
    finally {
      this.userStore.setLoading(false);
    }
  }

  async loadProjects() {
    let projects = await this.projectDataService.getAll({ active: true });
    this.userStore.setProjects(projects);
  }

  async loadRoutes(projectId: number) {
    let dto = {
      project: projectId,
      active: true
    };

    let routes = await this.routeDataService.getAll(dto);
    this.userStore.setRoutes(routes);
  }

  async changePassword(userUi: UserUI, password: string) {
    try {
      this.userStore.setModalLoading(true);

      let dto = {
        password
      } as ChangePasswordDto;

      await this.userDataService.changePassword(userUi.id, dto);
      this.notificationService.success(`Пароль для пользователя ${userUi.fullName} успешно изменён. Письмо с новым паролем было отправлено.`, userUi.id);
    }
    catch (exc) {
      this.notificationService.exception(exc);
    }
    finally {
      this.userStore.setModalLoading(false);
    }
  }

  private mapToModel(dto: UserDto, loggedUserId: number) {
    let item = dto as UserUI;

    if (dto.role) {
      item.roleDisplayName = dto.role.displayName;

      if (dto.project) {
        item.roleDisplayName += ' (' + dto.project.name + ')';
      }
    }

    switch (item.status.id) {
      case UserStatusEnum.Active:
        item.statusStyle = 'label-pill-success';
        item.canChangePassword = true;
        item.canToggleUser = item.id != loggedUserId;
        item.isActive = true;
        item.canConfirmUser = false;
        break;
      case UserStatusEnum.WaitingForConfirmation:
        item.statusStyle = 'label-pill-warn';
        item.canChangePassword = false;
        item.canToggleUser = false;
        item.isActive = false;
        item.canConfirmUser = true;
        break;
      case UserStatusEnum.Locked:
      case UserStatusEnum.Disabled:
        item.statusStyle = 'label-pill-error';
        item.canChangePassword = false;
        item.canToggleUser = true;
        item.isActive = false;
        item.canConfirmUser = false;
        break;
    }
     
    return item;
  }

  onDestroy() {
    this.userStore.reset();
  }
}
