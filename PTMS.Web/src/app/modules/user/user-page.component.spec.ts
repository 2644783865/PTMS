import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { componentsBaseImports } from '@app/test/test-imports';

import { UserPageComponent } from './user-page.component';
import { UserService } from './user.service';
import { authStubProvider } from '@app/test/auth-service.stub';

describe('UserPageComponent', () => {
  let component: UserPageComponent;
  let fixture: ComponentFixture<UserPageComponent>;
  const userServiceStub = {
    loadData: () => { }
  };

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: componentsBaseImports,
      providers: [
        { provide: UserService, useValue: userServiceStub },
        authStubProvider
      ],
      declarations: [UserPageComponent]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UserPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
