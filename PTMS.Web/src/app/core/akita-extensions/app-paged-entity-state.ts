import { ID, transaction } from '@datorama/akita';
import { AppEntityState, AppEntityStore, AppQueryEntity } from './app-entity-state';
import { Observable, of } from 'rxjs';
import { switchMap } from 'rxjs/operators';

export interface AppPaginationResponse<E> {
  currentPage: number;
  perPage: number;
  data: E[];
  total: number;
}

export interface AppPagedEntityState<T> extends AppEntityState<T> {
  modalLoading: boolean;
  currentPage: number;
  perPage: number;
  total: number;
}

export class AppPagedEntityStore<S extends AppPagedEntityState<E>, E, EntityID = ID>
  extends AppEntityStore<S, E, EntityID> {

  setPaginationResponse(pageResponse: AppPaginationResponse<E>) {
    this.update({
      currentPage: pageResponse.currentPage,
      perPage: pageResponse.perPage,
      total: pageResponse.total
    } as Partial<S>);

    this.set(pageResponse.data);
  }

  constructor() {
    super();
  }
}

export class AppPagedQueryEntity<S extends AppPagedEntityState<E>, E, EntityID = ID>
  extends AppQueryEntity<S, E, EntityID> {

  get paginationResponse$(): Observable<AppPaginationResponse<E>> {
    return this.selectAll()
      .pipe(switchMap(entities => {
        let value = this.getValue();

        let result = {
          currentPage: value.currentPage,
          perPage: value.perPage,
          total: value.total,
          data: entities
        } as AppPaginationResponse<E>;

        return of(result);
      }));
  }
  
  constructor(store: AppPagedEntityStore<S, E, EntityID>) {
    super(store);
  }
}
