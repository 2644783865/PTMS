import { Injectable, NgZone } from '@angular/core';
import { MatSnackBar, MatSnackBarConfig } from '@angular/material';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  constructor(
    private readonly snackBar: MatSnackBar,
    private readonly zone: NgZone
  ) {}
  
  info(message: string) {
    this.show(message, {
      panelClass: 'info-notification-overlay'
    });
  }

  success(message: string) {
    this.show(message, {
      panelClass: 'success-notification-overlay'
    });
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

  private show(message: string, configuration: MatSnackBarConfig) {
    let config = {
      duration: 8000,
      verticalPosition: 'top',
      horizontalPosition: 'center',
      ...configuration
    } as MatSnackBarConfig;

    this.zone.run(() => this.snackBar.open(message, null, config));
  }
}
