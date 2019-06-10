import { NgModule } from '@angular/core';
import { UpdatedRowDirective } from './notification-highlight.directive';

@NgModule({
  declarations: [
    UpdatedRowDirective
  ],
  exports: [
    UpdatedRowDirective
  ]
})
export class DirectivesModule { }
