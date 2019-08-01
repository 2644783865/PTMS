import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';

import { LayoutModule } from '@angular/cdk/layout';
import { MatButtonModule } from '@angular/material/button';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatMenuModule } from '@angular/material/menu';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatTableModule, MatDialogModule, MatListModule, MatSortModule } from '@angular/material/';

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
