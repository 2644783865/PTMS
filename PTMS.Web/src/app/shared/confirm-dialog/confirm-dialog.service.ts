import { Injectable } from '@angular/core';
import { MatDialog } from '@angular/material';
import { ConfirmDialogComponent } from './confirm-dialog.component';
import { ConfirmModalDialogData } from './confirm-dialog.model';

declare let Zone: any;

@Injectable({
  providedIn: 'root'
})
export class ConfirmDialogService {

  constructor(private dialog: MatDialog) {
  }

  openDialog(
    message: string,
    yesButtonText: string = 'Да',
    noButtonText: string = 'Нет'): Promise<void> {

    const promise = new Promise((resolve, reject) => {

      let data = {
        message,
        yesButtonText,
        noButtonText
      } as ConfirmModalDialogData;

      this.dialog.open(ConfirmDialogComponent, {
        width: '400px',
        data: data,
        autoFocus: false
      }).afterClosed().subscribe(result => {
        if (result) {
          resolve();
        }
        else {
          //small hack for zone.js to not log error in the console window
          Zone[Zone.__symbol__('ignoreConsoleErrorUncaughtError')] = true;
          reject();
          Zone[Zone.__symbol__('ignoreConsoleErrorUncaughtError')] = false;
        }
      });
    });

    const result = promise as unknown as Promise<void>;
    return result;
  }
}
