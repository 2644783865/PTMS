import { Component, Inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { ProviderDto } from '@app/core/dtos/ProviderDto';
import { Observable } from 'rxjs';
import { ObjectService } from '../object.service';
import { ObjectQuery, ObjectUI } from '../object.state';

@Component({
  selector: 'app-object-change-provider-dialog',
  templateUrl: 'object-change-provider-dialog.component.html',
})
export class ObjectChangeProviderDialogComponent {
  modalLoading$: Observable<boolean>;
  modalForm: FormGroup;
  providers$: Observable<ProviderDto[]>;

  constructor(
    private objectQuery: ObjectQuery,
    private objectService: ObjectService,
    private dialogRef: MatDialogRef<ObjectChangeProviderDialogComponent>,
    private fb: FormBuilder,
    @Inject(MAT_DIALOG_DATA) public vehicle: ObjectUI) { }

  ngOnInit() {
    this.modalLoading$ = this.objectQuery.modalLoading$;
    this.providers$ = this.objectQuery.providers$;

    this.modalForm = this.fb.group({
      provider: ['', Validators.required]
    });
  }

  async onSubmit() {
    if (this.modalForm.valid) {
      let provider = this.modalForm.get('provider').value as ProviderDto;
      await this.objectService.changeProvider(this.vehicle, provider);
      this.onClose();
    }    
  }

  onClose(): void {
    this.dialogRef.close();
  }
}
