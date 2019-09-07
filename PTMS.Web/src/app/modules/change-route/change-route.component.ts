import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormGroupDirective } from '@angular/forms';
import { ObjectDto, RouteDto } from '@app/core/dtos';
import { Observable } from 'rxjs';
import { ChangeRouteService } from './change-route.service';
import { ChangeRouteQuery } from './change-route.state';

@Component({
  selector: 'app-change-route-widget',
  templateUrl: './change-route.component.html'
})
export class ChangeRouteComponent implements OnInit {
  routeForm: FormGroup;
  vehicles$: Observable<ObjectDto[]>;
  routes$: Observable<RouteDto[]>;

  @ViewChild(FormGroupDirective) formGroupDirective: FormGroupDirective;
  
  constructor(
    private fb: FormBuilder,
    private changeRouteService: ChangeRouteService,
    private changeRouteQuery: ChangeRouteQuery) {
  }

  async ngOnInit() {
    this.routeForm = this.fb.group({
      vehicle: [''],
      newRoute: ['', Validators.required]
    });

    this.vehicles$ = this.changeRouteQuery.list$;
    this.routes$ = this.changeRouteQuery.routes$;

    await this.changeRouteService.loadRelatedData();
  }

  displayFn(vehicle: ObjectDto) {
    return vehicle ? `${vehicle.name} - ${vehicle.route.name}` : null;
  }

  search(plateNumber: string) {
    this.changeRouteService.search(plateNumber);
  }

  async onSave() {
    let formValue = this.routeForm.value;

    let vehicle = formValue.vehicle as ObjectDto;
    let newRoute = formValue.newRoute as RouteDto;

    let updatedItem = await this.changeRouteService.save(vehicle, newRoute);

    this.formGroupDirective.resetForm();
  }

  ngOnDestroy() {
    this.changeRouteService.onDestroy();
  }
}
