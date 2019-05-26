import { Component, Input } from '@angular/core';
import { AbstractControl, FormGroupDirective } from '@angular/forms';
import { defaultErrorMessages } from './default-error-messages';
import { combineLatest } from 'rxjs';

@Component({
  selector: 'app-input-error',
  templateUrl: './input-error.component.html'
})
export class AppInputErrorComponent {
  @Input() controlName: string;
  @Input() minlength: number;
  @Input() maxlength: number;
  @Input() customErrors: { [key: string]: string; }

  control: AbstractControl;
  errorMessage: string;

  constructor(
    private formGroup: FormGroupDirective
  ) { }

  ngOnInit() {
    this.control = this.formGroup.form.get(this.controlName);
  }

  showError() {
    return this.control.touched || this.formGroup.submitted;
  }

  onStatusChange() {
    if (this.control.invalid) {
      let errorName = Object.keys(this.control.errors)[0];

      this.errorMessage = this.customErrors && this.customErrors[errorName];
      this.errorMessage = this.errorMessage || defaultErrorMessages[errorName];

      if (errorName == 'maxlength') {
        this.errorMessage = this.errorMessage.replace('{0}', this.maxlength.toString());
      }

      if (errorName == 'minlength') {
        this.errorMessage = this.errorMessage.replace('{0}', this.minlength.toString());
      }
    }
    else {
      this.errorMessage = null;
      this.maxlength = null;
    }
  }

  ngAfterViewInit() {
    setTimeout(() => {
      this.onStatusChange();

      combineLatest(
        this.control.statusChanges,
      ).subscribe(this.onStatusChange.bind(this));
    });
  }
}
