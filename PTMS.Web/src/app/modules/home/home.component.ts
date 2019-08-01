import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { HomeQuery, ProjectStat, ProviderStat, RouteStat } from './home.state';
import { HomeService } from './home.service';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material';
import { formatDate } from '@angular/common';
import { ProjectDto } from '@app/core/dtos/ProjectDto';
import { KeyValuePair } from '@app/core/helpers/utils';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  private _updateInterval: number = 1000 * 10; //10 секунд
  private _intervalId;

  dataLoading$: Observable<boolean>;
  updateAt: string;
  routeStatFilters: FormGroup;
  projects$: Observable<ProjectDto[]>;
  intervalDropdown: KeyValuePair<string>[];

  statByProject$: Observable<ProjectStat[]>;
  statByProvider$: Observable<ProviderStat[]>;
  statByRoute$: Observable<RouteStat[]>;

  totalOnline$: Observable<number>;
  totalFact$: Observable<number>;
  totalPlanned$: Observable<number>;

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
    this.projects$ = this.homeQuery.projects$;

    this.statByProject$ = this.homeQuery.statByProject$;
    this.statByProvider$ = this.homeQuery.statByProvider$;
    this.statByRoute$ = this.homeQuery.statByRoute$;

    this.totalOnline$ = this.homeQuery.totalOnline$;
    this.totalFact$ = this.homeQuery.totalFact$
    this.totalPlanned$ = this.homeQuery.totalPlanned$;

    this.setFilters();
    
    await this.homeService.loadRelatedData();

    await this.loadReports();

    this.startUpdateInterval();
  }

  trackByRouteStat(index: number, routeStat: RouteStat) {
    return routeStat.route.id;
  }

  trackByProjectStat(index: number, projectStat: ProjectStat) {
    return projectStat.project.id;
  }

  ngOnDestroy() {
    this.clearInterval();
    this.homeService.onDestroy();
  }

  private setFilters() {
    this.intervalDropdown = [
      { key: 'последние 5 минут', value: '5' },
      { key: 'последние 10 минут', value: '10' },
      { key: 'последние 30 минут', value: '30' },
      { key: 'последний 1 час', value: '60' },
      { key: 'последний 3 часа', value: '180' },
      { key: 'за сегодня', value: 'today' },
      { key: 'за неделю', value: 'week' }
    ];

    let routeStatFilters = this.homeQuery.getValue().routeStatFilters;

    this.routeStatFilters = this.fb.group({
      showOnlyErrors: [routeStatFilters.showOnlyErrors],
      projectId: [routeStatFilters.projectId],
      intervalId: [routeStatFilters.intervalId]
    });

    this.routeStatFilters.get('intervalId').valueChanges.subscribe(_ => {
      this.clearInterval();

      window.setTimeout(() => {
        this.loadReports();
        this.startUpdateInterval();
      });
    });

    this.routeStatFilters.valueChanges.subscribe(newFilters => {
      this.homeService.setRouteStatFilters(newFilters);
    });
  }

  private async loadReports() {
    let intervalId = this.routeStatFilters.get('intervalId').value;

    await this.homeService.loadObjectsForReporting(intervalId);

    this.updateAt = `Обновлено в ${formatDate(new Date(), "HH:mm:ss", "ru")}`;
  }

  private startUpdateInterval() {
    let that = this;

    this.clearInterval();

    this._intervalId = setInterval(async _ => {
      that.loadReports();
    }, this._updateInterval);
  }

  private clearInterval() {
    if (this._intervalId) {
      clearInterval(this._intervalId);
    }
  }
}
