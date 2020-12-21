import { TestBed } from '@angular/core/testing';

import { HealthArticlesService } from './health-articles.service';

describe('HealthArticlesService', () => {
  let service: HealthArticlesService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(HealthArticlesService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
