import { Directive, ElementRef, Input } from '@angular/core';
import { Subscription } from 'rxjs';

@Directive({
  selector: '[ngShow]'
})
export class NgShowDirective {
  @Input() ngShow: boolean;

  constructor(private el: ElementRef) {
  }

  ngOnInit() {
    this.onUpdate();
  }

  ngOnChanges() {
    this.onUpdate();
  }

  private onUpdate() {
    let classList = this.el.nativeElement.classList;

    if (this.ngShow) {
      classList.remove("hidden");
    }
    else {
      classList.add("hidden");
    }
  }
}
