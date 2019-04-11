import { PaginatorPlugin, QueryEntity, PaginatorConfig, PaginationResponse, ID } from '@datorama/akita';
import { BehaviorSubject, combineLatest, Observable } from 'rxjs';
import { switchMap, distinctUntilChanged, tap, map } from 'rxjs/operators';

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
          const req = () => {
            return dataServiceFunc(page, pageSize, searchParams);
          }

          return this.getPage(req);
        })
      )
  }

  get onChange() {
    let paramsChange = combineLatest(
      this.pageChanges.pipe(distinctUntilChanged()),
      this.pageSizeChanges.pipe(distinctUntilChanged()),
      this.searchParamsChanges)
      .pipe(tap(_ => this.clearCache())
    );

    let storeUpdateChange = this.query.selectUpdatedEntityIds();

    return combineLatest(
        paramsChange,
        storeUpdateChange)
      .pipe(map(([p, s]) => p));
  }

  get pageSizeChanges() {
    return this.pageSize.asObservable();
  }

  get searchParamsChanges() {
    return this.searchParams.asObservable();
  }

  setPageParams(page: number, pageSize: number) {
    this.setPage(page);
    this.setPageSize(pageSize);
  }

  setPageSize(pageSize: number) {
    this.pageSize.next(pageSize);
  }

  setSearchParams(params: object) {
    this.setPage(1);
    this.searchParams.next(params);
  }
}
export const AppPaginator = AppPaginatorPlugin;
