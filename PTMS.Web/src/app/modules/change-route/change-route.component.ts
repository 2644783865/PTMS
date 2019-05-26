import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormGroupDirective } from '@angular/forms';
import { ObjectDto } from '@app/core/dtos/ObjectDto';
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

  @ViewChild(FormGroupDirective) formGroupDirective: FormGroupDirective;
  
  constructor(
    private fb: FormBuilder,
    private changeRouteService: ChangeRouteService,
    private changeRouteQuery: ChangeRouteQuery) {
  }

  ngOnInit() {
    this.routeForm = this.fb.group({
      vehicle: [''],
      newRouteName: ['', Validators.required, this.changeRouteService.routeValidator]
    });

    this.vehicles$ = this.changeRouteQuery.selectAll();
  }

  displayFn(vehicle: ObjectDto) {
    return vehicle ? `${vehicle.name} - ${vehicle.route.name}` : null;
  }

  search(plateNumber: string) {
    this.changeRouteService.search(plateNumber);
  }

  async onSave() {
    if (this.routeForm.valid) {
      let formValue = this.routeForm.value;

      let vehicle = formValue.vehicle as ObjectDto;
      let newRouteName = formValue.newRouteName as string;

      let updatedItem = await this.changeRouteService.save(vehicle, newRouteName);

      this.formGroupDirective.resetForm();

      this.routeForm.setValue({
        vehicle: updatedItem,
        newRouteName: ''
      });
    }
  }

  ngOnDestroy() {
    this.changeRouteService.onDestroy();
  }
}
