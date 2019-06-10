import { EntityState, EntityStore, ID, QueryEntity } from '@datorama/akita';

export interface AppEntityState<T> extends EntityState<T> {
  modalLoading: boolean;
}

export class AppEntityStore<S extends AppEntityState<E>, E, EntityID = ID>
  extends EntityStore<S, E, EntityID> {

  setModalLoading(modalLoading: boolean) {
    this.update({
      modalLoading
    } as Partial<S>);
  }

  constructor() {
    super();
  }
}

export class AppQueryEntity<S extends AppEntityState<E>, E, EntityID = ID>
  extends QueryEntity<S, E, EntityID> {

  get dataLoading$() {
    return this.selectLoading();
  }

  get modalLoading$() {
    return this.select(state => state.modalLoading);
  }

  constructor(store: AppEntityStore<S, E, EntityID>) {
    super(store);
  }
}
