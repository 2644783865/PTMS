import { Injectable } from '@angular/core';
import { AbstractControl, AsyncValidator, ValidationErrors } from '@angular/forms';
import { Observable, AsyncSubject, of } from 'rxjs';
import { map } from 'rxjs/operators';
import { RouteDataService } from '../data-services/route.data.service';
import { RouteDto } from '../dtos/RouteDto';

@Injectable({ providedIn: 'root' })
export class RouteHelper implements AsyncValidator {
  private _routes$: AsyncSubject<RouteDto[]>;

  constructor(private routeService: RouteDataService) {}

  get routes$(): Observable<RouteDto[]> {
    if (this._routes$ == null) {
      this._routes$ = new AsyncSubject<RouteDto[]>();

      this.routeService
        .getAll()
        .subscribe(res => {
          this._routes$.next(res);
          this._routes$.complete();
        });
    }

    return this._routes$.asObservable();
  }

  setRoutes(routes: RouteDto[]) {
    if (this._routes$ == null) {
      this._routes$ = new AsyncSubject<RouteDto[]>();
      this._routes$.next(routes);
      this._routes$.complete();
    }
    else {
      throw new Error("Routes are already initialized");
    }
  }

  validate(ctrl: AbstractControl): Observable<ValidationErrors | null> {
    return this.getRouteByName(ctrl.value)
      .pipe(
        map(route => route ? null : { routeNotExist: true })
      );
  }

  getRouteByName(routeName: string): Observable<RouteDto> {
    return this.routes$.pipe(
      map(routes => routes.find(r => r.name.toLowerCase() == routeName.toLowerCase()))
    );
  }

  onDestroy(): void {
    this._routes$ = null;
  }
}
