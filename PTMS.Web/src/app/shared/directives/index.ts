import { NgModule } from '@angular/core';
import { UpdatedRowDirective } from './notification-highlight.directive';
import { NgShowDirective } from './ng-show';
import { InlineFormSubmit } from './inline-form-submit';
import { AppFormSubmit } from './app-form-submit';

let directives = [
  UpdatedRowDirective,
  NgShowDirective,
  InlineFormSubmit,
  AppFormSubmit
]

@NgModule({
  declarations: directives,
  exports: directives
})
export class DirectivesModule { }
