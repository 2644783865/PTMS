import { FocusMonitor } from '@angular/cdk/a11y';
import { coerceBooleanProperty } from '@angular/cdk/coercion';
import { Component, ElementRef, EventEmitter, Input, OnDestroy, OnInit, Optional, Output, Self, HostListener } from '@angular/core';
import { ControlValueAccessor, FormControl, NgControl, FormControlName } from '@angular/forms';
import { MatFormFieldControl } from '@angular/material';
import { combineLatest, Observable, of, Subject, BehaviorSubject } from 'rxjs';
import { debounceTime, startWith, switchMap, map, skip } from 'rxjs/operators';

@Component({
  selector: 'app-autocomplete',
  templateUrl: 'autocomplete.component.html',
  providers: [{
    provide: MatFormFieldControl,
    useExisting: AutocompleteComponent
  }],
  host: {
    '[id]': 'id',
    '[attr.aria-describedby]': 'describedBy',
  }
})
export class AutocompleteComponent implements ControlValueAccessor, OnInit, OnDestroy, MatFormFieldControl<any> {
  static nextId = 0;

  autocompleteControl: FormControl
  stateChanges = new Subject<void>();
  valueChanges = new BehaviorSubject<Object | null>(null);
  focused = false;
  controlType = 'app-autocomplete';
  id = `app-autocomplete-${AutocompleteComponent.nextId++}`;
  describedBy = '';

  filteredOptions: Object[]
  displayFn: Function;
  onChange: Function;
  showSpinner: boolean;

  constructor(
    @Optional() @Self() public ngControl: NgControl,
    private focusMonitor: FocusMonitor,
    private elRef: ElementRef<HTMLElement>
  ) {
    this.displayFn = this.displayFn || this._defaultDisplayFn;
    this.autocompleteControl = new FormControl(null);

    if (this.ngControl != null) {
      this.ngControl.valueAccessor = this;
    }
  }

  get errorState(): boolean {
    if (this.ngControl) {
      let ngControl = this.ngControl as FormControlName;

      return ngControl.errors != null 
        && (ngControl.touched || ngControl.formDirective.submitted);
    }
    else {
      return false;
    }
  }

  get empty() {
    return !this.value;
  }

  get shouldLabelFloat() {
    return this.focused || !!this.autocompleteControl.value;
  }

  @Input()
  get displayWith(): string | Function {
    return this.displayFn;
  }
  set displayWith(value: string | Function) {
    value = value || 'name';
    
    if (typeof value == "string") {
      this.displayFn = x => x ? x[value as string] : null;
    }
    else {
      this.displayFn = value as Function;
    }

    this.stateChanges.next();
  }

  @Input()
  get options(): Observable<Object[]> { return this._options; }
  set options(value: Observable<Object[]>) {
    this._options = value;
    this.stateChanges.next();
  }
  private _options: Observable<Object[]>;

  @Input()
  get placeholder(): string { return this._placeholder; }
  set placeholder(value: string) {
    this._placeholder = value;
    this.stateChanges.next();
  }
  private _placeholder: string;

  @Input()
  get required(): boolean { return this._required; }
  set required(value: boolean) {
    this._required = coerceBooleanProperty(value);
    this.stateChanges.next();
  }
  private _required = false;

  @Input()
  get disabled(): boolean { return this._disabled; }
  set disabled(value: boolean) {
    this._disabled = coerceBooleanProperty(value);
    value ? this.autocompleteControl.disable() : this.autocompleteControl.enable();
    this.stateChanges.next();
  }
  private _disabled = false;

  @Input()
  get value(): Object | null {
    let value = this.autocompleteControl.value;

    if (typeof value == "string") {
      return null;
    }
    else {
      return value;
    }
  }
  set value(obj: Object | null) {
    this.autocompleteControl.setValue(obj);
    this.stateChanges.next();
  }

  @Output() search = new EventEmitter<string>();

  ngOnInit() {
    this.showSpinner = false;
    
    if (this.search.observers.length > 0) {
      this.autocompleteControl.valueChanges
        .pipe(debounceTime(150))
        .subscribe(newValue => {
          //If object is not selected
          if (!this.value) {
            this.showSpinner = true;
            this.search.emit(newValue);
          }
        });

      this.options.subscribe((options) => {
        this.filteredOptions = options;
        this.showSpinner = false;
      });
    }
    else {
      //Sync search - filter prepaid list
      combineLatest(
        this.autocompleteControl.valueChanges.pipe(startWith(null)),
        this.options)
        .pipe(
          switchMap(([inputValue, options]) => {
            let filteredOptions = this._filter(inputValue, options);
            return of(filteredOptions);
          })
        )
        .subscribe(val => {
          this.filteredOptions = val;
        });
    }

    this.autocompleteControl.valueChanges
      .pipe(map(x => this.value))
      .subscribe(newValue => {
        let previousValue = this.valueChanges.getValue();

        if (newValue != previousValue) {
          this.valueChanges.next(newValue);
        }
      });
  }

  ngAfterViewInit() {
    let input = this.elRef.nativeElement.querySelector('input');

    window.setTimeout(_ => {
      if (this.elRef.nativeElement.hasAttribute('cdkFocusInitial')) {
        input.focus();
      }
    });

    this.focusMonitor.monitor(input)
      .subscribe(origin => {
        this.focused = !!origin;
        this.stateChanges.next();
      });
  }

  @HostListener('document:click', ['$event.target'])
  public onOutsideClick(targetElement) {
      const clickedInside = this.elRef.nativeElement.contains(targetElement);
      if (!clickedInside) {
        //Check value on outside click
        if (!this.value && this.autocompleteControl.value && this.filteredOptions){
          let selectedValue = this.filteredOptions.find(option => 
            this.displayFn(option).toLowerCase() == this.autocompleteControl.value.toLowerCase());
    
          this.autocompleteControl.setValue(selectedValue);
        }
      }
  }

  onBlur() {
    setTimeout(() => {
      try {
        let formControl = this.ngControl.control as FormControl;
        formControl.markAsTouched();
      }
      catch (exc) {
        console.log(exc);
      }
    }, 100);
  }

  writeValue(delta: Object | null): void {
    this.autocompleteControl.setValue(delta);
    this.valueChanges.next(delta);
  }

  registerOnChange(fn: (v: any) => void): void {
    this.valueChanges
      .pipe(skip(1)) //skip first default value
      .subscribe(fn);
  }

  registerOnTouched(fn: () => void): void {
  }

  ngOnDestroy() {
    this.stateChanges.complete();
    this.valueChanges.complete();
    this.focusMonitor.stopMonitoring(this.elRef);
  }

  setDescribedByIds(ids: string[]) {
    this.describedBy = ids.join(' ');
  }

  onContainerClick(event: MouseEvent) {
    if ((event.target as Element).tagName.toLowerCase() != 'input') {
      this.elRef.nativeElement.querySelector('input')!.click();
    }
  }

  private _defaultDisplayFn(x) {
    return x ? x.name : null;
  }

  private _filter(inputValue: string, options: Object[]): Object[] {
    if (!inputValue || typeof inputValue !== 'string') {
      return options;
    }

    if (!options) {
      return [];
    }
    
    const filterValue = inputValue.toLowerCase();
    return options.filter(option => this.displayFn(option).toLowerCase().includes(filterValue));
  }
}
