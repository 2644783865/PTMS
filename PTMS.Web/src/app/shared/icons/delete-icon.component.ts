import { Component } from '@angular/core';

@Component({
  selector: 'app-delete-icon',
  template: `
<mat-icon class="icon-button icon-button-danger"
  matTooltip="Удалить" matTooltipPosition="above">
  delete
</mat-icon>
`
})
export class DeleteIconComponent {}
