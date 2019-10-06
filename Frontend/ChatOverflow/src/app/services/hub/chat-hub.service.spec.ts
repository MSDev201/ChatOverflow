import { TestBed } from '@angular/core/testing';

import { ChatHubService } from './chat-hub.service';

describe('ChatHubService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ChatHubService = TestBed.get(ChatHubService);
    expect(service).toBeTruthy();
  });
});
