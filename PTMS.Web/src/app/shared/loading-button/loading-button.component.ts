import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-loading-button',
  template: `
<button mat-raised-button
      [color]="color"
      [type]="type"
      class="loading-button"
      [class.active]="isLoading"
      [disabled]="isLoading">
  <ng-content></ng-content>
<mat-spinner diameter="19"></mat-spinner>
</button>`
})
export class LoadingButtonComponent {
  @Input() type: string;
  @Input() color: string;
  @Input() isLoading: boolean;
}
