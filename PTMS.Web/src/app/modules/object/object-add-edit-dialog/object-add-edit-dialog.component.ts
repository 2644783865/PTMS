import { Component, Inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { BlockTypeDto } from '@app/core/dtos/BlockTypeDto';
import { CarBrandDto } from '@app/core/dtos/CarBrandDto';
import { ProviderDto } from '@app/core/dtos/ProviderDto';
import { Observable } from 'rxjs';
import { ObjectService } from '../object.service';
import { ObjectQuery, ObjectUI } from '../object.state';

@Component({
  selector: 'app-object-add-edit-dialog',
  templateUrl: 'object-add-edit-dialog.component.html',
})
export class ObjectAddEditDialogComponent {
  modalLoading$: Observable<boolean>;
  modalForm: FormGroup;
  isNewVehicle: boolean;

  providers$: Observable<ProviderDto[]>;
  carBrands$: Observable<CarBrandDto[]>;
  blockTypes$: Observable<BlockTypeDto[]>;

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

    this.modalForm = this.fb.group({
      name: ['', Validators.required],
      providerId: ['', Validators.required],
      phone: ['', Validators.required],
      carBrand: [],
      yearRelease: [],
      blockNumber: [],
      blockTypeId: []
    });

    if (!this.isNewVehicle) {
      let vehicle = this.vehicle;

      this.modalForm.setValue({
        name: vehicle.name,
        providerId: vehicle.providerId,
        phone: vehicle.phone,
        carBrand: vehicle.carBrand || null,
        yearRelease: vehicle.yearRelease,
        blockNumber: vehicle.block ? vehicle.block.blockNumber : null,
        blockTypeId: vehicle.block ? vehicle.block.blockTypeId : null
      });
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
