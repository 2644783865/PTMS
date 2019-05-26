import { Component } from '@angular/core';

@Component({
  selector: 'app-menu-icon-dropdown',
  template: `
 <button mat-mini-fab [matMenuTriggerFor]="menu" color="primary">
    <mat-icon>menu</mat-icon>
  </button>
  <mat-menu #menu="matMenu" xPosition="before">
    <ng-content></ng-content>
  </mat-menu>
`
})
export class MenuIconDropdownComponent { }
