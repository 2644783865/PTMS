import { RouterTestingModule } from '@angular/router/testing';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { BrowserModule } from '@angular/platform-browser';
import { CoreModule } from '@app/core';
import { SharedModule } from '@app/shared';

export const componentsBaseImports = [
  BrowserAnimationsModule,
  BrowserModule,
  RouterTestingModule,
  CoreModule,
  SharedModule
]
