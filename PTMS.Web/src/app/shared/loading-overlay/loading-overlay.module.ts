import { NgModule } from '@angular/core';
import { LoadingOverlayComponent } from './loading-overlay.component';
import { LoadingBarModule } from '../loading-bar/loading-bar.module';

@NgModule({
  imports: [LoadingBarModule],
  declarations: [LoadingOverlayComponent],
  exports: [
    LoadingOverlayComponent
  ]
})
export class LoadingOverlayModule { }
