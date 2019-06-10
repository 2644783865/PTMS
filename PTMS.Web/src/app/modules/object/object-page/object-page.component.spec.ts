import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { PaginatorEvent } from '@app/shared/paginator/paginator.event';
import { authStubProvider } from '@app/test/auth-service.stub';
import { componentsBaseImports } from '@app/test/test-imports';
import { BehaviorSubject, of } from 'rxjs';
import { ObjectPageComponent } from './object-page.component';
import { ObjectService } from '../object.service';
import { PaginationResponse } from '@datorama/akita';
import { ObjectDto } from '@app/core/dtos/ObjectDto';
import { isElementVisible, getProgressBar, getTableRows } from '@app/test/test-utilities';

describe('ObjectPageComponent', () => {
  let component: ObjectPageComponent;
  let fixture: ComponentFixture<ObjectPageComponent>;
  let objectService: ObjectService;

  let serviceStub = {
    get pagination$() { return new BehaviorSubject([]).asObservable() },
    get isLoading$() { return new BehaviorSubject(false).asObservable() },
    
    ...jasmine.createSpyObj('ObjectService', ['loadPage', 'onDestroy'])
  }

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: componentsBaseImports,
      providers: [
        {
          provide: ObjectService,
          useValue: serviceStub
        },
        authStubProvider
      ],
      declarations: [ObjectPageComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ObjectPageComponent);
    component = fixture.componentInstance;
    objectService = TestBed.get(ObjectService);
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should have defined observables', () => {
    expect(component.pagination$).toBeTruthy();
    expect(component.dataLoading$).toBeTruthy();
  });

  it('should display transporter column', () => {
    expect(component.displayedColumns.includes('transporter')).toBeTruthy();
  });

  it('should call loadPage when onParamsChange called', () => {
    component.onParamsChange({} as PaginatorEvent);
    expect(objectService.loadPage).toHaveBeenCalledTimes(1);
  });

  it('should display loading bar if observer true', () => {
    component.dataLoading$ = of(true);
    fixture.detectChanges();
    const loadingBar = getProgressBar(fixture);
    expect(isElementVisible(loadingBar)).toBe(true);
  });

  it('should hide loading bar if observer false', () => {
    component.dataLoading$ = of(false);
    fixture.detectChanges();
    const loadingBar = getProgressBar(fixture);
    expect(isElementVisible(loadingBar)).toBe(false);
  });

  it('should display table with data', () => {
    component.pagination$ = of({
      currentPage: 1,
      perPage: 5,
      total: 2,
      data: [{ objectType: {}, route: {}, transporter: {} }, { objectType: {}, route: {}, transporter: {} }]
    } as PaginationResponse<ObjectDto>);

    fixture.detectChanges();

    let rows = getTableRows(fixture);
    expect(rows.length).toEqual(2);
  });

  it('should call onDestroy when ngOnDestroy called', () => {
    component.ngOnDestroy();
    expect(objectService.onDestroy).toHaveBeenCalled();
  });
});
