import { Injectable, Inject } from '@angular/core';
import { Observable } from 'rxjs';
import { PaginationResponse } from '@datorama/akita';
import { ObjectDto } from '@app/core/dtos/ObjectDto';
import { ObjectDataService } from '@app/core/data-services/object.data.service';
import { ObjectQuery, OBJECT_PAGINATOR, ObjectStore } from './object.state';
import { AppPaginatorPlugin } from '@app/core/paginator/app-paginator.plugin';

@Injectable()
export class ObjectService {
  public readonly isLoading$: Observable<boolean>;
  public readonly pagination$: Observable<PaginationResponse<ObjectDto>>;

  constructor(
    private objectStore: ObjectStore,
    @Inject(OBJECT_PAGINATOR) private paginatorRef: AppPaginatorPlugin<ObjectDto>,
    private objectDataService: ObjectDataService)
  {
    this.paginatorRef.setPageSize(5);

    this.isLoading$ = this.paginatorRef.isLoading$;
    this.pagination$ = this.paginatorRef.getDataObservable((page, pageSize) => {
      return this.objectDataService.getAll(page, pageSize);
    });    
  }  

  loadPage(page: number, pageSize: number) {
    this.paginatorRef.setPageParams(page, pageSize);
  }

  onDestroy() {
    this.paginatorRef.destroy({ clearCache: true, currentPage: 1 });
  }
}
