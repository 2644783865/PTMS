import { formatDate } from '@angular/common';

export interface KeyValuePair<T> {
  key: string;
  value: T
}

export function toDate(date: Date) {
  return formatDate(date, "yyyy-MM-dd", "en")
}

export function toDateTime(date: Date) {
  return formatDate(date, "yyyy-MM-dd HH:mm", "en")
}

export function toMidnightDateTime(date: Date) {
  let newDate = new Date(date);
  newDate.setHours(23, 59);
  return formatDate(newDate, "yyyy-MM-dd HH:mm", "en")
}

export function isNotNullOrEmpty(value: any) {
  return value !== undefined && value !== null && value !== '';
}
