import { NgModule } from '@angular/core';
import { LoadingBarComponent } from './loading-bar.component';
import { MatProgressBarModule } from '@angular/material';

@NgModule({
  imports: [MatProgressBarModule],
  declarations: [LoadingBarComponent],
  exports: [
    LoadingBarComponent
  ]
})
export class LoadingBarModule { }
