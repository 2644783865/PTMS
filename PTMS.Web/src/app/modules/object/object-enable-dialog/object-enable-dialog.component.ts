import { Component, Inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { ProjectDto } from '@app/core/dtos/ProjectDto';
import { Observable } from 'rxjs';
import { debounceTime } from 'rxjs/operators';
import { ObjectService } from '../object.service';
import { ObjectQuery, ObjectUI } from '../object.state';

@Component({
  templateUrl: 'object-enable-dialog.component.html',
})
export class ObjectEnableDialogComponent {
  modalLoading$: Observable<boolean>;
  modalForm: FormGroup;
  projectForSelectedRoute: ProjectDto;

  constructor(
    private objectQuery: ObjectQuery,
    private objectService: ObjectService,
    private dialogRef: MatDialogRef<ObjectEnableDialogComponent>,
    private fb: FormBuilder,
    @Inject(MAT_DIALOG_DATA) public vehicle: ObjectUI) { }

  ngOnInit() {
    this.modalLoading$ = this.objectQuery.modalLoading$;

    let routeName = (this.vehicle.route && this.vehicle.route.id > 0)
      ? this.vehicle.route.name
      : '';

    if (routeName) {
      this.projectForSelectedRoute = this.vehicle.project;
    }

    this.modalForm = this.fb.group({
      newRouteName: [routeName, Validators.required, this.objectService.routeValidator]
    });

    this.modalForm.get('newRouteName')
      .valueChanges
      .pipe(debounceTime(200))
      .subscribe(this.onRouteChange.bind(this));
  }

  async onSubmit() {
    if (this.modalForm.valid) {
      let newRouteName = this.modalForm.get('newRouteName').value as string;
      await this.objectService.enable(this.vehicle, newRouteName);
      this.onClose();
    }    
  }

  onClose(): void {
    this.dialogRef.close();
  }

  private onRouteChange(routeName: string) {
    this.projectForSelectedRoute = null;

    if (this.modalForm.get('newRouteName').invalid) {
      return;
    }

    this.objectService.getProjectByRouteName(routeName)
      .then(project => {
        this.projectForSelectedRoute = project;
      });
  }
}
