import { Component, Input, Output, EventEmitter } from '@angular/core';
import { PageEvent } from '@angular/material';
import { PaginatorEvent } from './paginator.event';

@Component({
  selector: 'app-paginator',
  templateUrl: './paginator.component.html'
})
export class PaginatorComponent {
  @Input() totalCount: number;
  @Input() pageSize: number;
  @Output() onChange = new EventEmitter<PaginatorEvent>();

  pageSizeOptions: number[] = [2, 5, 10, 25, 50];

  onPageChange(pageEvent: PageEvent) {
    let event = {
      page: pageEvent.pageIndex + 1,
      pageSize: pageEvent.pageSize
    } as PaginatorEvent;

    this.onChange.emit(event);
  }
}
