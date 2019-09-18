import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';

import { LayoutModule } from '@angular/cdk/layout';
import { MatTableModule, MatDialogModule, MatListModule, MatSortModule, MatButtonModule, MatToolbarModule, MatSelectModule, MatInputModule, MatProgressBarModule, MatProgressSpinnerModule, MatCardModule, MatSidenavModule, MatCheckboxModule, MatMenuModule, MatIconModule, MatSnackBarModule, MatTooltipModule } from '@angular/material/';

import { PaginatorModule } from './paginator/paginator.module';
import { PageHeaderModule } from './page-header/page-header.module';
import { IconModule } from './icons/icon.module';
import { AppInputErrorModule } from './input-error/input-error.module';
import { LoadingButtonModule } from './loading-button/loading-button.module';
import { LoadingBarModule } from './loading-bar/loading-bar.module';
import { LoadingOverlayModule } from './loading-overlay/loading-overlay.module';
import { AutocompleteModule } from './autocomplete/autocomplete.module';
import { DirectivesModule } from './directives';
import { PipesModule } from './pipes';
import { ConfirmDialogModule } from './confirm-dialog/confirm-dialog.module';
export * from './helpers';

let modules = [
  //angular
  CommonModule,
  ReactiveFormsModule,

  //material
  LayoutModule,
  MatButtonModule,
  MatToolbarModule,
  MatSelectModule,
  MatInputModule,
  MatProgressBarModule,
  MatProgressSpinnerModule,
  MatCardModule,
  MatSidenavModule,
  MatCheckboxModule,
  MatMenuModule,
  MatIconModule,
  MatSnackBarModule,
  MatTableModule,
  MatDialogModule,
  MatListModule,
  MatSortModule,
  MatTooltipModule,

  //app
  PaginatorModule,
  PageHeaderModule,
  IconModule,
  AppInputErrorModule,
  LoadingButtonModule,
  LoadingBarModule,
  LoadingOverlayModule,
  AutocompleteModule,
  DirectivesModule,
  PipesModule,
  ConfirmDialogModule
];

@NgModule({
  imports: modules,
  exports: modules
})
export class SharedModule { }
