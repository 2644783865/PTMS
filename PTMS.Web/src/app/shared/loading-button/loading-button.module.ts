import { NgModule } from '@angular/core';
import { LoadingButtonComponent } from './loading-button.component';
import { MatProgressSpinnerModule, MatButtonModule } from '@angular/material';

@NgModule({
  imports: [MatButtonModule, MatProgressSpinnerModule],
  declarations: [LoadingButtonComponent],
  exports: [
    LoadingButtonComponent
  ]
})
export class LoadingButtonModule { }
