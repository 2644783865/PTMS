import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { RouteFullDto } from '@app/core/dtos';
import { RouteService } from './route.service';
import { RouteQuery } from './route.state';
import { Router } from '@angular/router';

@Component({
  selector: 'app-route-page',
  templateUrl: './route-page.component.html'
})
export class RoutePageComponent implements OnInit {
  list$: Observable<RouteFullDto[]>;
  dataLoading$: Observable<boolean>;
  displayedColumns = ['name', 'project', 'status', 'controls'];

  constructor(
    private router: Router,
    private routeQuery: RouteQuery,
    private routeService: RouteService) { }

  ngOnInit() {
    this.list$ = this.routeQuery.list$;
    this.dataLoading$ = this.routeQuery.dataLoading$;

    this.routeService.loadData();
  }

  openAddEditDialog(route: RouteFullDto){
    if (route != null){
      this.router.navigateByUrl(`routes/${route.id}`);
    }
    else {
      this.router.navigateByUrl(`routes/add`);
    }
  }
  
  ngOnDestroy() {
    this.routeService.onDestroy();
  }
}
