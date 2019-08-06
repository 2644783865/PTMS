import { Component, Inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { ProjectDto } from '@app/core/dtos/ProjectDto';
import { Observable } from 'rxjs';
import { debounceTime } from 'rxjs/operators';
import { ObjectService } from '../object.service';
import { ObjectQuery, ObjectUI } from '../object.state';
import { RouteDto } from '@app/core/dtos/RouteDto';

@Component({
  templateUrl: 'object-enable-dialog.component.html',
})
export class ObjectEnableDialogComponent {
  modalLoading$: Observable<boolean>;
  modalForm: FormGroup;
  projectForSelectedRoute: ProjectDto;
  routes$: Observable<RouteDto[]>;

  constructor(
    private objectQuery: ObjectQuery,
    private objectService: ObjectService,
    private dialogRef: MatDialogRef<ObjectEnableDialogComponent>,
    private fb: FormBuilder,
    @Inject(MAT_DIALOG_DATA) public vehicle: ObjectUI) { }

  ngOnInit() {
    this.modalLoading$ = this.objectQuery.modalLoading$;
    this.routes$ = this.objectQuery.routes$;

    let route = (this.vehicle.route && this.vehicle.route.id > 0)
      ? this.vehicle.route
      : null;

    if (route) {
      this.projectForSelectedRoute = this.vehicle.project;
    }

    this.modalForm = this.fb.group({
      route: [route, Validators.required]
    });

    this.modalForm.get('route')
      .valueChanges
      .pipe(debounceTime(200))
      .subscribe(this.onRouteChange.bind(this));
  }

  async onSubmit() {
    if (this.modalForm.valid) {
      let route = this.modalForm.get('route').value as RouteDto;
      await this.objectService.enable(this.vehicle, route);
      this.onClose();
    }    
  }

  onClose(): void {
    this.dialogRef.close();
  }

  private onRouteChange(route: RouteDto) {
    this.projectForSelectedRoute = null;

    if (this.modalForm.get('route').invalid) {
      return;
    }

    this.objectService.getProjectByRouteName(route.name)
      .then(project => {
        this.projectForSelectedRoute = project;
      });
  }
}
