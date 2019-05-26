import { NgModule } from '@angular/core';
import { AppInputErrorComponent } from './input-error.component';
import { MatInputModule } from '@angular/material/input';

@NgModule({
  imports: [MatInputModule],
  declarations: [AppInputErrorComponent],
  exports: [
    AppInputErrorComponent
  ]
})
export class AppInputErrorModule { }
