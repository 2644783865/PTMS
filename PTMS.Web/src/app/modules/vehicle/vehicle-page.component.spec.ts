import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { PaginatorEvent } from '@app/shared/paginator/paginator.event';
import { authStubProvider } from '@app/test/auth-service.stub';
import { componentsBaseImports } from '@app/test/test-imports';
import { BehaviorSubject, of } from 'rxjs';
import { VehiclePageComponent } from './vehicle-page.component';
import { VehicleService } from './vehicle.service';
import { PaginationResponse } from '@datorama/akita';
import { VehicleDto } from '@app/core/dtos/VehicleDto';
import { isElementVisible, getProgressBar, getTableRows } from '@app/test/test-utilities';

describe('VehiclePageComponent', () => {
  let component: VehiclePageComponent;
  let fixture: ComponentFixture<VehiclePageComponent>;
  let vehicleService: VehicleService;

  let serviceStub = {
    get pagination$() { return new BehaviorSubject([]).asObservable() },
    get isLoading$() { return new BehaviorSubject(false).asObservable() },
    
    ...jasmine.createSpyObj('VehicleService', ['loadPage', 'onDestroy'])
  }

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: componentsBaseImports,
      providers: [
        {
          provide: VehicleService,
          useValue: serviceStub
        },
        authStubProvider
      ],
      declarations: [VehiclePageComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VehiclePageComponent);
    component = fixture.componentInstance;
    vehicleService = TestBed.get(VehicleService);
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
    expect(vehicleService.loadPage).toHaveBeenCalledTimes(1);
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
      data: [{ vehicleType: {}, route: {}, transporter: {} }, { vehicleType: {}, route: {}, transporter: {} }]
    } as PaginationResponse<VehicleDto>);

    fixture.detectChanges();

    let rows = getTableRows(fixture);
    expect(rows.length).toEqual(2);
  });

  it('should call onDestroy when ngOnDestroy called', () => {
    component.ngOnDestroy();
    expect(vehicleService.onDestroy).toHaveBeenCalled();
  });
});
