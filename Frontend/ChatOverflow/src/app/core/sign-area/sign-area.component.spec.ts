import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SignAreaComponent } from './sign-area.component';

describe('SignAreaComponent', () => {
  let component: SignAreaComponent;
  let fixture: ComponentFixture<SignAreaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SignAreaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SignAreaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
