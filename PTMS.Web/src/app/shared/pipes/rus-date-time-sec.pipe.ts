import { Pipe, PipeTransform } from '@angular/core';
import { formatDate } from '@angular/common';

@Pipe({
  name: 'RusDateTimeSec'
})
export class RusDateTimeSecPipe implements PipeTransform {
  transform(date: string | Date): string {
    if (date) {
      var result = formatDate(date, "dd.MM.yyyy HH:mm:ss", "ru");
      return result;
    }
    else {
      return '-';
    }
  }
}
