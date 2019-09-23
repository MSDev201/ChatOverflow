import { TestBed } from '@angular/core/testing';

import { UserSignService } from './user-sign.service';

describe('UserSignService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: UserSignService = TestBed.get(UserSignService);
    expect(service).toBeTruthy();
  });
});
