import { TestBed } from '@angular/core/testing';

import { WatchLaterService } from './watch-later.service';

describe('WatchLaterService', () => {
  let service: WatchLaterService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(WatchLaterService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
