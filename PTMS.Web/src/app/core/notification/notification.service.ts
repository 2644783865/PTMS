import { Injectable, NgZone } from '@angular/core';
import { MatSnackBar, MatSnackBarConfig } from '@angular/material';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  constructor(
    private readonly snackBar: MatSnackBar,
    private readonly zone: NgZone
  ) { }

  updatedEntityId$ = new BehaviorSubject<string | number>(null);
  configTimeout = 8000;
  
  info(message: string) {
    this.show(message, {
      panelClass: 'info-notification-overlay'
    });
  }

  success(message: string, updatedEntityId: number | string = null) {
    this.show(message, {
      panelClass: 'success-notification-overlay'
    });

    this.setUpdateEntityId(updatedEntityId);
  }

  warn(message: string) {
    this.show(message, {
      panelClass: 'warning-notification-overlay'
    });
  }

  error(message: string) {
    this.show(message, {
      panelClass: 'error-notification-overlay'
    });
  }

  exception(exception) {
    let errorMessage = '';

    if (exception.error instanceof ErrorEvent) {
      // client-side error
      errorMessage = `Error: ${exception.error.message}`;
    } else {
      // server-side error
      errorMessage = exception.message || 'Произошла ошибка. Приносим извинения за неудобства';
    }

    this.error(errorMessage);
  }

  setUpdateEntityId(updatedEntityId: number | string) {
    if (updatedEntityId) {
      this.updatedEntityId$.next(updatedEntityId);

      window.setTimeout(() => this.updatedEntityId$.next(null), this.configTimeout);
    }
  }

  private show(message: string, configuration: MatSnackBarConfig) {
    let config = {
      duration: this.configTimeout,
      verticalPosition: 'top',
      horizontalPosition: 'center',
      ...configuration
    } as MatSnackBarConfig;

    this.zone.run(() => this.snackBar.open(message, null, config));
  }
}
