import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { RouteDto } from '@app/core/dtos/RouteDto';
import { RouteService } from './route.service';
import { AuthService } from '@app/core/auth/auth.service';
import { RoleEnum } from '@app/core/enums/role.enum';

@Component({
  selector: 'app-route-page',
  templateUrl: './route-page.component.html'
})
export class RoutePageComponent implements OnInit {
  list$: Observable<RouteDto[]>;
  dataLoading$: Observable<boolean>;
  private readonly allColumns = ['name', 'controls'];
  displayedColumns: string[];
  canAdd: boolean;

  constructor(
    private routeService: RouteService,
    private authService: AuthService) { }

  ngOnInit() {
    this.list$ = this.routeService.list$;
    this.dataLoading$ = this.routeService.isLoading$;

    let isAdmin = this.authService.isInRole(RoleEnum.Administrator);

    if (isAdmin) {
      this.displayedColumns = this.allColumns;
      this.canAdd = true;
    }
    else {
      this.displayedColumns = this.allColumns.filter(x => x != 'controls');
      this.canAdd = false;
    }

    this.routeService.loadData();
  }
  
  ngOnDestroy() {
    this.routeService.onDestroy();
  }
}
