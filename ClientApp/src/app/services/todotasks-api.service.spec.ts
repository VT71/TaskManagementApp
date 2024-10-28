import { TestBed } from '@angular/core/testing';

import { TodotasksApiService } from './todotasks-api.service';

describe('TodotasksApiService', () => {
  let service: TodotasksApiService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(TodotasksApiService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
