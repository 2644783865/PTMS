import { NgModule, } from '@angular/core';
import localeRu from '@angular/common/locales/ru';
import { registerLocaleData } from '@angular/common';
import { RusDatePipe } from './rus-date.pipe';
import { RusDateTimePipe } from './rus-date-time.pipe';
import { NaPipe } from './na.pipe';
import { RusDateTimeSecPipe } from './rus-date-time-sec.pipe';

// the second parameter 'fr' is optional
registerLocaleData(localeRu);

@NgModule({
  declarations: [
    RusDatePipe,
    RusDateTimePipe,
    RusDateTimeSecPipe,
    NaPipe
  ],
  exports: [
    RusDatePipe,
    RusDateTimePipe,
    RusDateTimeSecPipe,
    NaPipe
  ]
})
export class PipesModule {}
