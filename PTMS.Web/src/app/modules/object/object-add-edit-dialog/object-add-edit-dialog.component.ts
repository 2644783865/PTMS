import { Component, Inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { Observable } from 'rxjs';
import { ProviderDto, CarBrandDto, BlockTypeDto, RouteDto } from '@app/core/dtos';
import { ObjectQuery, ObjectUI } from '../object.state';
import { ObjectService } from '../object.service';
import { requiredIf } from '@app/core/validation';

@Component({
  selector: 'app-object-add-edit-dialog',
  templateUrl: 'object-add-edit-dialog.component.html',
})
export class ObjectAddEditDialogComponent {
  modalLoading$: Observable<boolean>;
  modalForm: FormGroup;
  isNewVehicle: boolean;
  showRouteRequired: boolean = false;

  providers$: Observable<ProviderDto[]>;
  carBrands$: Observable<CarBrandDto[]>;
  blockTypes$: Observable<BlockTypeDto[]>;
  routes$: Observable<RouteDto[]>;

  constructor(
    private objectQuery: ObjectQuery,
    private objectService: ObjectService,
    private dialogRef: MatDialogRef<ObjectAddEditDialogComponent>,
    private fb: FormBuilder,
    @Inject(MAT_DIALOG_DATA) public vehicle: ObjectUI) { }

  ngOnInit() {
    this.modalLoading$ = this.objectQuery.modalLoading$;
    this.isNewVehicle = !this.vehicle;

    this.providers$ = this.objectQuery.providers$;
    this.carBrands$ = this.objectQuery.carBrands$;
    this.blockTypes$ = this.objectQuery.blockTypes$;
    this.routes$ = this.objectQuery.routes$;

    this.modalForm = this.fb.group({
      name: ['', Validators.required],
      providerId: ['', Validators.required],
      phone: ['', Validators.required],
      carBrand: [],
      route: [],
      yearRelease: [],
      blockNumber: [],
      blockTypeId: ['', requiredIf('blockNumber')]
    });

    if (!this.isNewVehicle) {
      let vehicle = this.vehicle;

      this.modalForm.setValue({
        name: vehicle.name,
        providerId: vehicle.providerId,
        phone: vehicle.phone,
        route: vehicle.route || null,
        carBrand: vehicle.carBrand || null,
        yearRelease: vehicle.yearRelease,
        blockNumber: vehicle.block ? vehicle.block.blockNumber : null,
        blockTypeId: vehicle.block ? vehicle.block.blockTypeId : null
      });

      if (!vehicle.objOutput) {
        this.modalForm.get('route').setValidators(Validators.required);
        this.showRouteRequired = true;
      }
    }
  }

  async onSubmit() {
    if (this.modalForm.valid) {
      let vehicleId = this.isNewVehicle ? null : this.vehicle.id;
      let result = await this.objectService.addOrUpdate(vehicleId, this.modalForm.value);

      if (result) {
        this.onClose();
      }
    }
  }

  onClose(): void {
    this.dialogRef.close();
  }
}
