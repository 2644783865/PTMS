import { NgModule, } from '@angular/core';
import localeRu from '@angular/common/locales/ru';
import { registerLocaleData } from '@angular/common';
import { RusDatePipe } from './rus-date.pipe';
import { RusDateTimePipe } from './rus-date-time.pipe';

// the second parameter 'fr' is optional
registerLocaleData(localeRu);

@NgModule({
  declarations: [
    RusDatePipe,
    RusDateTimePipe
  ],
  exports: [
    RusDatePipe,
    RusDateTimePipe
  ]
})
export class PipesModule {}
