import { PaginatorPlugin, QueryEntity, PaginatorConfig, PaginationResponse } from '@datorama/akita';
import { BehaviorSubject, combineLatest, Observable } from 'rxjs';
import { switchMap, distinctUntilChanged } from 'rxjs/operators';

export class AppPaginatorPlugin<E> extends PaginatorPlugin<E> {
  private pageSize: BehaviorSubject<number>;
  private searchParams: BehaviorSubject<object>;

  constructor(protected query: QueryEntity<any, E>, public config: PaginatorConfig = {}) {
    super(query, config);

    this.pageSize = new BehaviorSubject(10);
    this.searchParams = new BehaviorSubject(null);
  }

  getDataObservable(dataServiceFunc: (page: number, pageSize: number, searchParams: object) => Observable<PaginationResponse<E>>) {
    return this.onChange
      .pipe(
        switchMap(([page, pageSize, searchParams]) => {
          const req = () => dataServiceFunc(page, pageSize, searchParams);
          return this.getPage(req);
        })
      )
  }

  get onChange() {
    return combineLatest(
      this.pageChanges.pipe(distinctUntilChanged()),
      this.pageSizeChanges.pipe(distinctUntilChanged()),
      this.searchParamsChanges);
  }

  get pageSizeChanges() {
    return this.pageSize.asObservable();
  }

  get searchParamsChanges() {
    return this.searchParams.asObservable();
  }

  setPageSize(pageSize: number) {
    this.clearCache();
    this.pageSize.next(pageSize);
  }

  setSearchParams(params: object) {
    this.clearCache();
    this.searchParams.next(params);
  }

  setAll(page: number, pageSize: number, searchParams: object = null) {
    this.setPage(page);
    this.setPageSize(pageSize);
    this.setSearchParams(searchParams);
  }
}
export const AppPaginator = AppPaginatorPlugin;
