import { Directive, ElementRef, Input } from '@angular/core';
import { Subscription } from 'rxjs';
import { NotificationService } from '@app/core/notification/notification.service';

@Directive({
  selector: '[appUpdatedRow]'
})
export class UpdatedRowDirective {
  @Input('appUpdatedRow') rowId: number | string;

  private subscription: Subscription;
  private entityId: string | number;
  private hasClass: boolean = false;

  constructor(
    private el: ElementRef,
    private notificationService: NotificationService) {
  }

  ngOnInit() {
    this.subscription = this.notificationService.updatedEntityId$
      .subscribe((eId) => {
        this.entityId = eId;
        this.onUpdate();
      });
  }

  ngOnChanges() {
    this.onUpdate();
  }

  private onUpdate() {
    if (this.rowId == this.entityId) {
      this.el.nativeElement.classList.add("table-row-updated");
      this.hasClass = true;
    }
    else if (this.hasClass) {
      this.el.nativeElement.classList.remove("table-row-updated");
      this.hasClass = false;
    }
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }
}
