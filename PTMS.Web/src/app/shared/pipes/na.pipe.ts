import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'NA'
})
export class NaPipe implements PipeTransform {
  transform(item: string | number): string | number {
    if (item !== null && item !== undefined) {
      return item;
    }
    else {
      return '-';
    }
  }
}
