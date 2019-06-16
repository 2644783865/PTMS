import { Injectable } from '@angular/core';
import { AppPagedEntityState, AppPagedEntityStore, AppPagedQueryEntity } from '@app/core/akita-extensions/app-paged-entity-state';
import { ObjectDto } from '@app/core/dtos/ObjectDto';
import { ProjectDto } from '@app/core/dtos/ProjectDto';
import { StoreConfig } from '@datorama/akita';
import { ProviderDto } from '@app/core/dtos/ProviderDto';
import { CarBrandDto } from '@app/core/dtos/CarBrandDto';
import { CarTypeDto } from '@app/core/dtos/CarTypeDto';

export interface ObjectUI extends ObjectDto {
  canChangeRoute: boolean;
  canChangeProvider: boolean;
  canEnable: boolean;
  canDisable: boolean;

  showMenu: boolean;
}

export interface ObjectState extends AppPagedEntityState<ObjectUI> {
  projects: ProjectDto[];
  providers: ProviderDto[];
  carBrands: CarBrandDto[];
  carTypes: CarTypeDto[];
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
    carTypes: CarTypeDto[]) {

    this.update({
      projects,
      providers,
      carBrands,
      carTypes
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

  constructor(protected store: ObjectStore) {
    super(store);
  }
}
