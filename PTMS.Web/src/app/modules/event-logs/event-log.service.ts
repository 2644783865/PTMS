import { Injectable } from '@angular/core';
import { EventLogDataService } from '@app/core/data-services';
import { EventLogStore } from './event-log.state';
import { PaginatorEvent } from '@app/shared/paginator/paginator.event';
import { toDate, toMidnightDateTime } from '@app/core/helpers';

@Injectable()
export class EventLogService {
  constructor(
    private eventLogStore: EventLogStore,
    private eventLogDataService: EventLogDataService)
  {
  }  

  async loadData() {
    this.eventLogStore.setLoading(true);
    let operatons = await this.eventLogDataService.getOperations();
    this.eventLogStore.setRelatedData(operatons);
  }

  async loadPage(event: PaginatorEvent, searchParams: any) {
    let page = event ? event.page : 1;
    let pageSize = event ? event.pageSize : 50;

    let dto = this.getFiltersDto(searchParams);

    this.eventLogStore.setLoading(true);
    let result = await this.eventLogDataService.getAll(page, pageSize, dto);

    this.eventLogStore.setPaginationResponse(result);
    this.eventLogStore.setLoading(false);
  }

  onDestroy() {
    this.eventLogStore.reset();
  }

  private getFiltersDto(searchParams: any) {
    let dto = {
      ...searchParams
    };

    if (searchParams.startDate) {
      dto.startDate = toDate(searchParams.startDate);
    }

    if (searchParams.endDate) {
      dto.endDate = toMidnightDateTime(searchParams.endDate);
    }

    return dto;
  }
}
