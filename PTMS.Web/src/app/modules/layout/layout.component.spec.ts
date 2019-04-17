import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { componentsBaseImports } from '@app/test/test-imports';
import { authStubProvider } from '@app/test/auth-service.stub';

import { LayoutComponent } from './layout.component';

describe('LayoutComponent', () => {
  let component: LayoutComponent;
  let fixture: ComponentFixture<LayoutComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [...componentsBaseImports],
      providers: [authStubProvider],
      declarations: [LayoutComponent]
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LayoutComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
