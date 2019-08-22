import { Component, Input } from '@angular/core';
import { FormGroupDirective, AbstractControl } from '@angular/forms';

@Component({
  selector: 'app-input-clear-icon',
  template: `
  <button mat-button
          [class.hidden]="!showIcon"
          mat-icon-button
          aria-label="Clear"
          (click)="onClick($event)"
          matTooltip="Очистить поле"
          matTooltipPosition="above"
          type="button"
          tabindex="-1">
    <mat-icon>close</mat-icon>
  </button>
`
})
export class InputClearIconComponent {
  @Input() controlName: string;
  control: AbstractControl;
  errorMessage: string;

  constructor(
    private formGroup: FormGroupDirective
  ) { }

  ngOnInit() {
    this.control = this.formGroup.form.get(this.controlName);
  }

  get showIcon() {
    let value = this.control.value;
    let hasValue = value !== null && value !== undefined && value !== '';
    return hasValue;
  }

  onClick($event) {
    this.control.setValue(null);
    $event.stopPropagation();
  }
}
