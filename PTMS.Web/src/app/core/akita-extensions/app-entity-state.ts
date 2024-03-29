import { EntityState, EntityStore, ID, QueryEntity, hasEntity, IDS } from '@datorama/akita';

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

  getValue(): S {
    return this._value();
  }

  addOrUpdate(id: ID, entity: E) {
    let isUpdate = hasEntity(this._value().entities, id);

    let ids: IDS = id;

    if (isUpdate) {
      this.update(ids, entity);
    }
    else {
      this.add(entity, {
        prepend: true
      });
    }
  }

  constructor(initialState: Partial<S> = {}) {
    super(initialState);
  }
}

export class AppQueryEntity<S extends AppEntityState<E>, E, EntityID = ID>
  extends QueryEntity<S, E, EntityID> {

  get list$() {
    return this.selectAll();
  }

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
