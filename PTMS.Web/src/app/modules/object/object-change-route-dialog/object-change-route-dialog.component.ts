import { Component, Inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { Observable } from 'rxjs';
import { ObjectDto } from '@app/core/dtos/ObjectDto';
import { ObjectQuery } from '../object.state';
import { ObjectService } from '../object.service';

@Component({
  selector: 'app-object-change-route-dialog',
  templateUrl: 'object-change-route-dialog.component.html',
})
export class ObjectChangeRouteDialogComponent {
  modalLoading$: Observable<boolean>;
  modalForm: FormGroup;

  constructor(
    private objectQuery: ObjectQuery,
    private objectService: ObjectService,
    private dialogRef: MatDialogRef<ObjectChangeRouteDialogComponent>,
    private fb: FormBuilder,
    @Inject(MAT_DIALOG_DATA) public vehicle: ObjectDto) { }

  ngOnInit() {
    this.modalLoading$ = this.objectQuery.modalLoading$;

    this.modalForm = this.fb.group({
      newRouteName: ['', Validators.required, this.objectService.routeValidator]
    });
  }

  async onSubmit() {
    if (this.modalForm.valid) {
      let newRouteName = this.modalForm.get('newRouteName').value as string;
      await this.objectService.changeRoute(this.vehicle, newRouteName);
      this.onClose();
    }    
  }

  onClose(): void {
    this.dialogRef.close();
  }
}
