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
  @Input() set currentPage(value: number) {
    this.pageIndex = value - 1;
  };
  @Output() onChange = new EventEmitter<PaginatorEvent>();

  pageSizeOptions: number[] = [2, 5, 10, 25, 50];
  pageIndex = 0;

  onPageChange(pageEvent: PageEvent) {
    let event = {
      page: pageEvent.pageIndex + 1,
      pageSize: pageEvent.pageSize
    } as PaginatorEvent;

    this.onChange.emit(event);

    let element = document.querySelector('mat-sidenav-content');
    element.scrollTo(0, 0);
  }
}
