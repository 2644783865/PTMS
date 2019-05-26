import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-loading-overlay',
  template: `
<app-loading-bar [isLoading]="isLoading"></app-loading-bar>
<div [class.loading-overlay]="isLoading">
  <ng-content></ng-content>
</div>
`
})
export class LoadingOverlayComponent {
  @Input() isLoading: boolean;
}
