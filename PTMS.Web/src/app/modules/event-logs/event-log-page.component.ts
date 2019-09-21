import { Component, OnInit } from '@angular/core';
import { Observable, merge } from 'rxjs';
import { ActivatedRoute } from '@angular/router';
import { EventLogDto, NamedEntityDto } from '@app/core/dtos';
import { AppPaginationResponse } from '@app/core/akita-extensions';
import { EventLogQuery } from './event-log.state';
import { EventLogService } from './event-log.service';
import { FormBuilder, FormGroup } from '@angular/forms';
import { PaginatorEvent } from '@app/shared/paginator/paginator.event';
import { debounceTime } from 'rxjs/operators';
import { isNotNullOrEmpty } from '@app/core/helpers';

@Component({
  templateUrl: './event-log-page.component.html',
  styleUrls: ['./event-log-page.component.scss']
})
export class EventLogPageComponent implements OnInit {
  operations$: Observable<NamedEntityDto[]>;

  pagination$: Observable<AppPaginationResponse<EventLogDto>>;
  dataLoading$: Observable<boolean>;  
  filters: FormGroup;
  displayedColumns = [ 'operation', 'entity', 'timeStamp', 'user', 'message', 'fields'];

  constructor(
    private route: ActivatedRoute,
    private eventLogQuery: EventLogQuery,
    private eventLogService: EventLogService,
    private fb: FormBuilder) { }

  async ngOnInit() {
    this.operations$ = this.eventLogQuery.operations$;
    this.pagination$ = this.eventLogQuery.paginationResponse$;
    this.dataLoading$ = this.eventLogQuery.dataLoading$;

    let entityId = this.route.snapshot.queryParams.entityId;
    this.initFilters(entityId);

    await this.eventLogService.loadData();

    this.search();
  }
  
  search(event: PaginatorEvent = null) {
    window.setTimeout(() => this.eventLogService.loadPage(event, this.filters.value));
  }

  getFieldValue(value: string) {
    return isNotNullOrEmpty(value) ? value : 'NULL';
  }

  ngOnDestroy() {
    this.eventLogService.onDestroy();
  }

  private initFilters(entityId: number) {
    this.filters = this.fb.group({
      eventEnum: [],
      entityId: [entityId],
      startDate: [],
      endDate: [],
      orderBy: ['desc']
    });
    
    merge(
      this.filters.get('entityId').valueChanges
    )
      .pipe(debounceTime(400))
      .subscribe(_ => this.search());

    merge(
      this.filters.get('eventEnum').valueChanges,
      this.filters.get('startDate').valueChanges,
      this.filters.get('endDate').valueChanges
    ).subscribe(_ => this.search());
  }
}
