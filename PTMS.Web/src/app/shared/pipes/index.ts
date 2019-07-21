import { NgModule, } from '@angular/core';
import localeRu from '@angular/common/locales/ru';
import { registerLocaleData } from '@angular/common';
import { RusDatePipe } from './rus-date.pipe';
import { RusDateTimePipe } from './rus-date-time.pipe';
import { NaPipe } from './na.pipe';

// the second parameter 'fr' is optional
registerLocaleData(localeRu);

@NgModule({
  declarations: [
    RusDatePipe,
    RusDateTimePipe,
    NaPipe
  ],
  exports: [
    RusDatePipe,
    RusDateTimePipe,
    NaPipe
  ]
})
export class PipesModule {}
