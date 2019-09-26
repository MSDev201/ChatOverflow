import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateGroupChatPageComponent } from './create-group-chat-page.component';

describe('CreateGroupChatPageComponent', () => {
  let component: CreateGroupChatPageComponent;
  let fixture: ComponentFixture<CreateGroupChatPageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CreateGroupChatPageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateGroupChatPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
