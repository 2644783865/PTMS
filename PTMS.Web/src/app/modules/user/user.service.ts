import { Injectable } from '@angular/core';
import { ProjectDataService } from '@app/core/data-services/project.data.service';
import { UserDataService } from '@app/core/data-services/user.data.service';
import { ChangePasswordDto } from '@app/core/dtos/ChangePasswordDto';
import { ConfirmUserDto } from '@app/core/dtos/ConfirmUserDto';
import { UserDto } from '@app/core/dtos/UserDto';
import { UserStatusEnum } from '@app/core/enums/user-status.enum';
import { NotificationService } from '@app/core/notification/notification.service';
import { UserStore, UserUI } from './user.state';
import { AuthService } from '@app/core/auth/auth.service';

@Injectable()
export class UserService {

  constructor(
    private userStore: UserStore,
    private userDataService: UserDataService,
    private projectDataService: ProjectDataService,
    private notificationService: NotificationService,
    private authService: AuthService)
  {
  }  

  async loadData() {
    let [roles, data] = await Promise.all([
      this.userDataService.getRoles().toPromise(),
      this.userDataService.getAll().toPromise()
    ]);

    let loggedUserId = this.authService.userId;
    let result = data.map(user => this.mapToModel(user, loggedUserId));

    this.userStore.set(result);
    this.userStore.setRoles(roles);
    this.userStore.setLoading(false);
  }

  async confirmUser(userId: number, formData: any) {
    try {
      this.userStore.setModalLoading(true);

      let dto = {
        roleId: formData.roleId,
        projectId: formData.project!.id
      } as ConfirmUserDto;

      let userDto = await this.userDataService.confirmUser(userId, dto).toPromise();
      let userUi = this.mapToModel(userDto, this.authService.userId);

      this.userStore.update(userUi.id, userUi);
      this.userStore.setModalLoading(false);
      this.notificationService.success(`Пользователь '${userUi.fullName}' успешно подтверждён.`);
    }
    catch (exc) {
      this.notificationService.error(exc.error.message);
    }
    finally {
      this.userStore.setModalLoading(false);
    }
  }

  async toggleAccount(user: UserUI) {
    try {
      this.userStore.setLoading(true);
      let userDto = await this.userDataService.toggleUser(user.id).toPromise();
      let userUi = this.mapToModel(userDto, this.authService.userId);

      this.userStore.update(userUi.id, userUi);

      this.notificationService.success(`Пользователь '${userUi.fullName}' успешно ${user.isActive ? 'заблокирован' : 'разблокирован'}.`);
    }
    catch (exc) {
      this.notificationService.error(exc.error.message);
    }
    finally {
      this.userStore.setLoading(false);
    }
  }

  async loadProjects() {
    let projects = await this.projectDataService.getAll().toPromise();
    this.userStore.setProjects(projects);
  }

  async changePassword(userUi: UserUI, password: string) {
    try {
      this.userStore.setModalLoading(true);

      let dto = {
        password
      } as ChangePasswordDto;

      await this.userDataService.changePassword(userUi.id, dto).toPromise();

      this.userStore.setModalLoading(false);
      this.notificationService.success(`Пароль для пользователя ${userUi.fullName} успешно изменён. Письмо с новым паролем было отправлено.`);
    }
    catch (exc) {
      this.notificationService.error(exc.error.message);
    }
    finally {
      this.userStore.setModalLoading(false);
    }
  }

  private mapToModel(dto: UserDto, loggedUserId: number) {
    let item = dto as UserUI;

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
