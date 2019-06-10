import { Injectable } from '@angular/core';
import { AppPagedEntityState, AppPagedEntityStore, AppPagedQueryEntity } from '@app/core/akita-extensions/app-paged-entity-state';
import { ObjectDto } from '@app/core/dtos/ObjectDto';
import { ProjectDto } from '@app/core/dtos/ProjectDto';
import { StoreConfig } from '@datorama/akita';

export interface ObjectState extends AppPagedEntityState<ObjectDto> {
  projects: ProjectDto[];
}

@Injectable()
@StoreConfig({
  idKey: 'ids',
  name: 'objects',
  resettable: true
})
export class ObjectStore extends AppPagedEntityStore<ObjectState, ObjectDto> {
  setProjects(projects: ProjectDto[]) {
    this.update({
      projects
    });
  }

  constructor() {
    super();
  }
}

@Injectable()
export class ObjectQuery extends AppPagedQueryEntity<ObjectState, ObjectDto> {
  projects$ = this.select(s => s.projects);

  constructor(protected store: ObjectStore) {
    super(store);
  }
}
