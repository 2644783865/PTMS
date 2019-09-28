import { Component, Inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { Observable } from 'rxjs';
import { BusStationDto } from '@app/core/dtos';
import { BusStationQuery } from '../bus-station.state';
import { BusStationService } from '../bus-station.service';

@Component({
  templateUrl: 'bus-station-add-edit-dialog.component.html',
})
export class BusStationAddEditDialogComponent {
  modalLoading$: Observable<boolean>;
  modalForm: FormGroup;

  constructor(
    private busStationQuery: BusStationQuery,
    private busStationService: BusStationService,
    private dialogRef: MatDialogRef<BusStationAddEditDialogComponent>,
    private fb: FormBuilder,
    @Inject(MAT_DIALOG_DATA) public station: BusStationDto) { }

  ngOnInit() {
    this.modalLoading$ = this.busStationQuery.modalLoading$;

    let station = this.station;

    this.modalForm = this.fb.group({
      id: [],
      name: ['', Validators.required],
      longitude: ['', Validators.required],
      latitude: ['', Validators.required],
      azimuth: ['', Validators.required]
    });

    if (this.station) {
      this.modalForm.setValue(station);
    }
  }

  async onSubmit() {
    await this.busStationService.save(this.modalForm.value);
    this.onClose();
  }

  onClose(): void {
    this.dialogRef.close();
  }
}
