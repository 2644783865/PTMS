import { Directive, ElementRef, Input } from '@angular/core';
import { Subscription } from 'rxjs';
import { NotificationService } from '@app/core/notification/notification.service';

@Directive({
  selector: '[appUpdatedRow]'
})
export class UpdatedRowDirective {
  @Input('appUpdatedRow') rowId: number | string;

  private subscription: Subscription;

  constructor(
    private el: ElementRef,
    private notificationService: NotificationService) {
  }

  ngOnInit() {
    this.subscription = this.notificationService.updatedEntityId$
      .subscribe(this.onUpdate.bind(this));
  }

  private onUpdate(entityId: number | string) {
    if (this.rowId == entityId) {
      this.el.nativeElement.classList.add("table-row-updated");
    }
    else {
      this.el.nativeElement.classList.remove("table-row-updated");
    }
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }
}
