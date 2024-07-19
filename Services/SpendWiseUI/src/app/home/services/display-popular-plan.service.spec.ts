import { TestBed } from '@angular/core/testing';

import { DisplayPopularPlanService } from './display-popular-plan.service';

describe('DisplayPopularPlanService', () => {
  let service: DisplayPopularPlanService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(DisplayPopularPlanService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
