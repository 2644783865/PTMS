import { ComponentFixture } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

export function getProgressBar<T>(fixture: ComponentFixture<T>) : DebugElement {
  return fixture.debugElement.query(By.css('mat-progress-bar'));
}

export function getTableGrid<T>(fixture: ComponentFixture<T>): DebugElement {
  return fixture.debugElement.query(By.css('mat-table'));
}

export function getTableRows<T>(fixture: ComponentFixture<T>): DebugElement[] {
  return fixture.debugElement.queryAll(By.css('mat-table mat-row'));
}

export function isElementVisible(debugElement: DebugElement): boolean {
  let elem = window.getComputedStyle(debugElement.nativeElement);
  return !(elem.visibility == 'hidden' || elem.display == 'none');
}
