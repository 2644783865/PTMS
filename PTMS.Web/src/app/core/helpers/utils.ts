import { formatDate } from '@angular/common';

export interface KeyValuePair<T> {
  key: string;
  value: T
}

export function toDate(date: Date) {
  return formatDate(date, "yyyy-MM-dd", "en")
}
