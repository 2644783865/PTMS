import { Directive, Output, EventEmitter, HostListener } from '@angular/core';
import { FormGroupDirective } from '@angular/forms';

@Directive({
  selector: '[inlineFormSubmit]'
})
export class InlineFormSubmit {
  @Output() inlineFormSubmit = new EventEmitter<object>();

  constructor(
    private formGroup: FormGroupDirective
  ) { }

  @HostListener('click', ['$event'])
  onClick(event) {
    (this.formGroup as any).submitted = true;

    if (this.formGroup.valid) {
      this.inlineFormSubmit.emit();
    }
  }
}
