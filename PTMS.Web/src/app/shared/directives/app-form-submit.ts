import { Directive, Output, EventEmitter, HostListener } from '@angular/core';
import { FormGroupDirective } from '@angular/forms';

@Directive({
  selector: '[appSubmit]'
})
export class AppFormSubmit {
  @Output() appSubmit = new EventEmitter<object>();

  constructor(
    private formGroup: FormGroupDirective
  ) { }

  @HostListener('submit', ['$event'])
  onClick(event) {
    (this.formGroup as any).submitted = true;

    if (this.formGroup.valid) {
      this.appSubmit.emit();
    }
  }
}
