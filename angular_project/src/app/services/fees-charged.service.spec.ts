import { TestBed } from '@angular/core/testing';

import { FeesChargedService } from './fees-charged.service';

describe('FeesChargedService', () => {
  let service: FeesChargedService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(FeesChargedService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
