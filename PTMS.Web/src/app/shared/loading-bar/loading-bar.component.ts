import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-loading-bar',
  template: `
<mat-progress-bar mode="indeterminate" [class.visibility-hidden]="!isLoading"></mat-progress-bar>
`
})
export class LoadingBarComponent {
  @Input() isLoading: boolean;
}
