import { Pipe, PipeTransform } from '@angular/core';
import { formatDate } from '@angular/common';

@Pipe({
  name: 'RusDate'
})
export class RusDatePipe implements PipeTransform {
  transform(date: string | Date): string {
    if (date) {
      var result = formatDate(date, "dd.MM.yyyy", "ru");
      return result;
    }
    else {
      return '-';
    }
  }
}
