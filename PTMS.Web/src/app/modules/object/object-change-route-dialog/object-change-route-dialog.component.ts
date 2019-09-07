import { Component, Inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { Observable } from 'rxjs';
import { ObjectService } from '../object.service';
import { ObjectQuery, ObjectUI } from '../object.state';
import { RouteDto } from '@app/core/dtos';
import { map } from 'rxjs/operators';

@Component({
  selector: 'app-object-change-route-dialog',
  templateUrl: 'object-change-route-dialog.component.html',
})
export class ObjectChangeRouteDialogComponent {
  modalLoading$: Observable<boolean>;
  modalForm: FormGroup;
  routes$: Observable<RouteDto[]>;
  showProjects: boolean;

  constructor(
    private objectQuery: ObjectQuery,
    private objectService: ObjectService,
    private dialogRef: MatDialogRef<ObjectChangeRouteDialogComponent>,
    private fb: FormBuilder,
    @Inject(MAT_DIALOG_DATA) public vehicle: ObjectUI) { }

  ngOnInit() {
    this.modalLoading$ = this.objectQuery.modalLoading$;
    this.routes$ = this.objectQuery.routes$
      .pipe(map(array => {
        return array ? array.filter(x => x.routeActive) : null;
      }));

    this.modalForm = this.fb.group({
      newRoute: ['', Validators.required]
    });
  }

  async onSubmit() {
    let newRoute = this.modalForm.get('newRoute').value as RouteDto;
    await this.objectService.changeRoute(this.vehicle, newRoute);
    this.onClose();   
  }

  onClose(): void {
    this.dialogRef.close();
  }
}
