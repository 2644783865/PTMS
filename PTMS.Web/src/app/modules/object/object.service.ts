import { Injectable } from '@angular/core';
import { ObjectStore, ObjectUI } from './object.state';
import { ObjectDataService, ProjectDataService, ProviderDataService, CarBrandDataService, CarTypeDataService, BlockTypeDataService, RouteDataService } from '@app/core/data-services';
import { RouteHelper } from '@app/core/helpers';
import { NotificationService } from '@app/core/notification';
import { AuthService } from '@app/core/auth';
import { ConfirmDialogService } from '@app/shared/confirm-dialog/confirm-dialog.service';
import { RoleEnum } from '@app/core/enums';
import { PaginatorEvent } from '@app/shared/paginator/paginator.event';
import { AppPaginationResponse } from '@app/core/akita-extensions';
import { RouteDto, ObjectAddEditRequestDto, ObjectDto } from '@app/core/dtos';

@Injectable()
export class ObjectService {
  constructor(
    private objectStore: ObjectStore,
    private objectDataService: ObjectDataService,
    private projectDataService: ProjectDataService,
    private providerDataService: ProviderDataService,
    private carBrandDataService: CarBrandDataService,
    private carTypeDataService: CarTypeDataService,
    private blockTypeDataService: BlockTypeDataService,
    private routeDataService: RouteDataService,
    private routeHelper: RouteHelper,
    private notificationService: NotificationService,
    private authService: AuthService,
    private confirmDialogService: ConfirmDialogService)
  {
  }

  get isDispatcher(): boolean {
    return this.authService.isInRole(RoleEnum.Dispatcher);
  }

  get isTransporter(): boolean {
    return this.authService.isInRole(RoleEnum.Transporter, RoleEnum.Mechanic);
  }

  get canAddVehicle(): boolean {
    return this.authService.isInRole(RoleEnum.Administrator, RoleEnum.Dispatcher);
  }

  get routeValidator() {
    return this.routeHelper.validate.bind(this.routeHelper);
  }

  async loadPage(event: PaginatorEvent, searchParams: any) {
    let page = event ? event.page : 1;
    let pageSize = event ? event.pageSize : 50;

    let dto = {
      ...searchParams,
      project: searchParams.project && searchParams.project.id,
      carBrand: searchParams.carBrand && searchParams.carBrand.id,
      active: this.getActiveFlag(searchParams.active),
      format: 'pure'
    };

    this.objectStore.setLoading(true);
    let response = await this.objectDataService.getAll(page, pageSize, dto).toPromise();

    let result = response as AppPaginationResponse<ObjectUI>;
    result.data = response.data.map(item => this.mapToModel(item));

    this.objectStore.setPaginationResponse(result);
    this.objectStore.setLoading(false);
  }

  async loadRelatedData() {
    let isTransporter = this.isTransporter;

    let [projects, providers, carBrands, carTypes, blockTypes, routes ] = await Promise.all([
      !isTransporter ? this.projectDataService.getAll().toPromise() : Promise.resolve([]),
      !isTransporter ? this.providerDataService.getAll().toPromise() : Promise.resolve([]),
      this.carBrandDataService.getAll().toPromise(),
      this.carTypeDataService.getAll().toPromise(),
      !isTransporter ? this.blockTypeDataService.getAll().toPromise() : Promise.resolve([]),
      this.routeDataService.getAll({ active: true }).toPromise()
    ]);

    carBrands.forEach(x => {
      x.carType = carTypes.find(y => y.id == x.carTypeId);
    });
    
    this.objectStore.setRelatedData(
      projects,
      providers,
      carBrands,
      carTypes,
      blockTypes,
      routes);

    this.routeHelper.setRoutes(routes);
  }

  async getProjectByRouteName(routeName: string) {
    let route = await this.routeHelper
      .getRouteByName(routeName)
      .toPromise();

    if (route) {
      let result = await this.projectDataService.getByRouteId(route.id).toPromise();
      return result;
    }
    else {
      return null;
    }
  }

  async changeRoute(vehicle: ObjectUI, newRouteName: string) {
    try {
      this.objectStore.setModalLoading(true);

      let newRoute = await this.routeHelper
        .getRouteByName(newRouteName)
        .toPromise()
            
      let updateItem = await this.objectDataService.changeRoute(vehicle.id, newRoute.id).toPromise();

      this.objectStore.update(updateItem.id, this.mapToModel(updateItem));

      this.notificationService.success(`Маршрут ТС номер ${updateItem.name} был успешно изменён`, updateItem.id);

      return updateItem;
    }
    catch (exc) {
      this.notificationService.exception(exc);
    }
    finally {
      this.objectStore.setModalLoading(false);
    }
  }

  async enable(vehicle: ObjectUI, newRoute: RouteDto) {
    try {
      this.objectStore.setModalLoading(true);

      let updateItem = await this.objectDataService.enable(vehicle.id, newRoute.id).toPromise();

      this.objectStore.update(updateItem.id, this.mapToModel(updateItem));

      this.notificationService.success(`ТС номер ${updateItem.name} был успешно введён в эксплуатацию`, updateItem.id);

      return updateItem;
    }
    catch (exc) {
      this.notificationService.exception(exc);
    }
    finally {
      this.objectStore.setModalLoading(false);
    }
  }

  async disable(vehicle: ObjectUI) {
    let message = `Вы действительно хотите вывести из
эксплуатации ТС номер ${vehicle.name} - ${vehicle.route.name} маршрут?`;

    await this.confirmDialogService.openDialog(message);

    try {
      this.objectStore.setLoading(true);

      let updateItem = await this.objectDataService.disable(vehicle.id).toPromise();

      this.objectStore.update(updateItem.id, this.mapToModel(updateItem));

      this.notificationService.success(`ТС номер ${updateItem.name} был успешно выведен из эксплуатации`, updateItem.id);

      return updateItem;
    }
    catch (exc) {
      this.notificationService.exception(exc);
    }
    finally {
      this.objectStore.setLoading(false);
    }
  }

  async addOrUpdate(vehicleId: number, formValue: any) {
    try {
      this.objectStore.setModalLoading(true);

      let dto = formValue as ObjectAddEditRequestDto;
      dto.carBrandId = formValue.carBrand ? formValue.carBrand.id : null;
      dto.routeId = formValue.route ? formValue.route.id : null;

      let updateItem = vehicleId > 0
        ? await this.objectDataService.update(vehicleId, dto).toPromise()
        : await this.objectDataService.add(dto).toPromise();

      this.objectStore.addOrUpdate(updateItem.id, this.mapToModel(updateItem));

      this.notificationService.success(`ТС номер ${updateItem.name} был успешно ${vehicleId > 0 ? 'отредактирован' : 'добавлен'}`, updateItem.id);

      return updateItem;
    }
    catch (exc) {
      this.notificationService.exception(exc);
    }
    finally {
      this.objectStore.setModalLoading(false);
    }
  }

  onDestroy() {
    this.objectStore.reset();
  }

  private getActiveFlag(value: number): boolean {
    if (value == 1) {
      return true;
    }
    else if (value == 0) {
      return false;
    }
    else {
      return null;
    }
  }

  private mapToModel(item: ObjectDto): ObjectUI {
    let vehicle = item as ObjectUI;

    vehicle.canUpdate = this.authService.isInRole(RoleEnum.Administrator, RoleEnum.Dispatcher);
    vehicle.canChangeRoute = !vehicle.objOutput && this.isTransporter;
    vehicle.canEnable = vehicle.objOutput && this.authService.isInRole(RoleEnum.Administrator, RoleEnum.Dispatcher);
    vehicle.canDisable = !vehicle.objOutput && this.authService.isInRole(RoleEnum.Administrator, RoleEnum.Dispatcher);

    vehicle.showMenu = vehicle.canChangeRoute
      || vehicle.canUpdate
      || vehicle.canEnable
      || vehicle.canDisable;

    let state = this.objectStore.getValue();

    if (state.routes.length > 0) {
      vehicle.route = state.routes.find(x => x.id == vehicle.lastRout);
    }

    if (state.projects.length > 0) {
      vehicle.project = state.projects.find(x => x.id == vehicle.projId);
    }

    if (state.providers.length > 0) {
      vehicle.provider = state.providers.find(x => x.id == vehicle.providerId);
    }

    if (state.blockTypes.length > 0 && vehicle.block) {
      vehicle.block.blockType = state.blockTypes.find(x => x.id == vehicle.block.blockTypeId);
    }

    vehicle.carBrand = state.carBrands.find(x => x.id == vehicle.carBrandId);

    return vehicle;
  }
}
