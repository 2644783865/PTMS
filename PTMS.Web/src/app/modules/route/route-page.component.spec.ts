import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { componentsBaseImports } from '@app/test/test-imports';

import { RoutePageComponent } from './route-page.component';
import { RouteService } from './route.service';
import { authStubProvider } from '@app/test/auth-service.stub';

describe('RoutePageComponent', () => {
  let component: RoutePageComponent;
  let fixture: ComponentFixture<RoutePageComponent>;
  const routeServiceStub = {
    loadData: () => { }
  };

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: componentsBaseImports,
      providers: [
        { provide: RouteService, useValue: routeServiceStub },
        authStubProvider
      ],
      declarations: [RoutePageComponent]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RoutePageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
