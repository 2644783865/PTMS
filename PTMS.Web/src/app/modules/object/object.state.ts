import { Injectable } from '@angular/core';
import { AppPagedEntityState, AppPagedEntityStore, AppPagedQueryEntity } from '@app/core/akita-extensions/app-paged-entity-state';
import { ObjectDto, ProjectDto, ProviderDto, CarBrandDto, CarTypeDto, BlockTypeDto, RouteDto } from '@app/core/dtos';
import { StoreConfig } from '@datorama/akita';


export interface ObjectUI extends ObjectDto {
  canUpdate: boolean;
  canChangeRoute: boolean;
  canEnable: boolean;
  canDisable: boolean;

  showMenu: boolean;
}

export interface ObjectState extends AppPagedEntityState<ObjectUI> {
  projects: ProjectDto[];
  providers: ProviderDto[];
  carBrands: CarBrandDto[];
  carTypes: CarTypeDto[];
  blockTypes: BlockTypeDto[];
  routes: RouteDto[]
}

@Injectable()
@StoreConfig({
  idKey: 'id',
  name: 'objects',
  resettable: true
})
export class ObjectStore extends AppPagedEntityStore<ObjectState, ObjectUI> {
  
  setRelatedData(
    projects: ProjectDto[],
    providers: ProviderDto[],
    carBrands: CarBrandDto[],
    carTypes: CarTypeDto[],
    blockTypes: BlockTypeDto[],
    routes: RouteDto[]) {

    this.update({
      projects,
      providers,
      carBrands,
      carTypes,
      blockTypes,
      routes
    });
  }

  constructor() {
    super();
  }
}

@Injectable()
export class ObjectQuery extends AppPagedQueryEntity<ObjectState, ObjectUI> {
  projects$ = this.select(s => s.projects);
  providers$ = this.select(s => s.providers);
  carBrands$ = this.select(s => s.carBrands);
  carTypes$ = this.select(s => s.carTypes);
  blockTypes$ = this.select(s => s.blockTypes);
  routes$ = this.select(s => s.routes);

  constructor(protected store: ObjectStore) {
    super(store);
  }
}
