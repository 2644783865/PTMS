import { MatDatepickerModule, MatNativeDateModule, DateAdapter, MAT_DATE_FORMATS } from '@angular/material';
import { NgModule } from '@angular/core';
import { RusDateAdapter } from './rus-date-adapter';

@NgModule({
    imports: [
        MatDatepickerModule,
        MatNativeDateModule
    ],
    exports: [
        MatDatepickerModule,
        MatNativeDateModule
    ],
    providers: [
      { provide: DateAdapter, useClass: RusDateAdapter },
      { 
          provide: MAT_DATE_FORMATS, 
          useValue: {
            parse: {
                dateInput: {month: 'numeric', year: 'numeric', day: 'numeric'}
            },
            display: {
                dateInput: { month: 'numeric', year: 'numeric', day: 'numeric' },                
                monthYearLabel: {year: 'numeric', month: 'short'},
                dateA11yLabel: {year: 'numeric', month: 'long', day: 'numeric'},
                monthYearA11yLabel: {year: 'numeric', month: 'long'},
            }
        }
      }
    ]
  })
  export class DatePickerModule { }