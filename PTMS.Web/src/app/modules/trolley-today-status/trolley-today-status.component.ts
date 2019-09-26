import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { TrolleybusTodayStatusService } from './trolley-today-status.service';
import { TrolleybusStatusQuery } from './trolley-today-status.state';
import { TrolleybusTodayStatusDto } from '@app/core/dtos';

@Component({
  templateUrl: './trolley-today-status.component.html',
  styleUrls: ['./trolley-today-status.component.scss']
})
export class TrolleyTodayStatusComponent implements OnInit {
  list$: Observable<TrolleybusTodayStatusDto[]>;
  dataLoading$: Observable<boolean>;
  displayedColumns = ['name', 'route', 'place', 'coordTime', 'controls'];

  constructor(
    private trolleybusStatusQuery: TrolleybusStatusQuery,
    private trolleybusTodayStatusService: TrolleybusTodayStatusService) { }

  ngOnInit() {
    this.list$ = this.trolleybusStatusQuery.list$;
    this.dataLoading$ = this.trolleybusStatusQuery.dataLoading$;

    this.trolleybusTodayStatusService.loadData();
  }

  async changeRoute(status: TrolleybusTodayStatusDto){
    await this.trolleybusTodayStatusService.changeRoute(status);
  }
  
  ngOnDestroy() {
    this.trolleybusTodayStatusService.onDestroy();
  }
}
