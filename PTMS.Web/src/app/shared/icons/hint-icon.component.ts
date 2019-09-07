import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-hint-icon',
  template: `
<mat-icon matTooltip="{{hint}}" matTooltipPosition="above"
          class="icon-button icon-button-primary">
  help
</mat-icon>
`
})
export class HintIconComponent {
  @Input() hint: string;
}
