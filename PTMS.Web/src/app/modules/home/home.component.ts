import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { HomeQuery, ProjectStat, ProviderStat, RouteStat } from './home.state';
import { HomeService } from './home.service';
import { FormBuilder } from '@angular/forms';
import { MatDialog } from '@angular/material';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  private _updateInterval: number = 1000 * 60; //1 минута
  private _intervalId;

  dataLoading$: Observable<boolean>;
  updateAt: string;

  statByProject$: Observable<ProjectStat[]>;
  statByProvider$: Observable<ProviderStat[]>;
  statByRoute$: Observable<RouteStat[]>;

  displayedColumnsProject = ['project', 'onlineNumber'];
  displayedColumnsProvider = ['provider', 'onlineNumber'];
  displayedColumnsRoute = ['route', 'onlineNumber'];
  
  constructor(
    private homeQuery: HomeQuery,
    private homeService: HomeService,
    private fb: FormBuilder,
    private dialog: MatDialog) { }

  async ngOnInit() {
    this.dataLoading$ = this.homeQuery.dataLoading$;

    this.statByProject$ = this.homeQuery.statByProject$;
    this.statByProvider$ = this.homeQuery.statByProvider$;
    this.statByRoute$ = this.homeQuery.statByRoute$;
    
    await this.homeService.loadRelatedData();

    await this.loadReports();

    this.startUpdateInterval();
  }

  ngOnDestroy() {
    if (this._intervalId) {
      clearInterval(this._intervalId);
    }

    this.homeService.onDestroy();
  }

  private async loadReports() {
    await this.homeService.loadObjectsForReporting(10);

    let minutes = new Date().getMinutes();
    this.updateAt = `Обновлено в ${new Date().getHours()}:${minutes < 10 ? '0' + minutes : minutes}`;
  }

  private startUpdateInterval() {
    let that = this;

    this._intervalId = setInterval(async _ => {
      that.loadReports();
    }, this._updateInterval);
  }
}
