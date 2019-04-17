import { TestBed } from '@angular/core/testing';
import { Type } from '@angular/core';

export function shouldCreate<T>(cl: Type<T>) {
  const fixture = TestBed.createComponent(cl);
  const component = fixture.debugElement.componentInstance;
  expect(component).toBeTruthy();
}
