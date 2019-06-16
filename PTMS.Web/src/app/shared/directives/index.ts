import { NgModule } from '@angular/core';
import { UpdatedRowDirective } from './notification-highlight.directive';
import { NgShowDirective } from './ng-show';

let directives = [
  UpdatedRowDirective,
  NgShowDirective
]

@NgModule({
  declarations: directives,
  exports: directives
})
export class DirectivesModule { }
