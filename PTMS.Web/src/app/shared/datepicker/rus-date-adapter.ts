import {NativeDateAdapter} from '@angular/material';
import {Injectable} from '@angular/core';
import { Platform } from '@angular/cdk/platform';

@Injectable()
export class RusDateAdapter extends NativeDateAdapter {
    constructor(){
        super("ru-RU", new Platform())
    }

    getFirstDayOfWeek(): number {
        return 1;
    }
}