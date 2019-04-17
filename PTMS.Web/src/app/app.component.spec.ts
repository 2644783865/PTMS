import { TestBed, async } from '@angular/core/testing';
import { AppComponent } from './app.component';
import { LayoutModule } from './modules/layout/layout.module';
import { componentsBaseImports } from '@app/test/test-imports';

describe('AppComponent', () => {
  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [
        ...componentsBaseImports,
        LayoutModule
      ],
      declarations: [
        AppComponent
      ],
    }).compileComponents();
  }));

  it('should create the app', () => {
    const fixture = TestBed.createComponent(AppComponent);
    const app = fixture.debugElement.componentInstance;
    expect(app).toBeTruthy();
  });
});
