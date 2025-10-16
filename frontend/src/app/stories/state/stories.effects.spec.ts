import { TestBed } from '@angular/core/testing';
import { provideMockActions } from '@ngrx/effects/testing';
import { Observable, of, throwError } from 'rxjs';
import { StoriesEffects } from './stories.effects';
import { StoriesService } from '../stories.service';
import { loadStories, loadStoriesSuccess, loadStoriesFailure } from './stories.feature';
import { Story } from '../story.model';

describe('StoriesEffects', () => {
  let actions$: Observable<any>;
  let effects: StoriesEffects;
  let storiesService: jasmine.SpyObj<StoriesService>;

  beforeEach(() => {
    const spy = jasmine.createSpyObj('StoriesService', ['getStories']);

    TestBed.configureTestingModule({
      providers: [
        StoriesEffects,
        provideMockActions(() => actions$),
        { provide: StoriesService, useValue: spy }
      ]
    });

    effects = TestBed.inject(StoriesEffects);
    storiesService = TestBed.inject(StoriesService) as jasmine.SpyObj<StoriesService>;
  });

  it('should dispatch loadStoriesSuccess on successful loadStories', (done) => {
    const mockStories: Story[] = [{ id: 1, title: 'title', url: 'url' }];
    storiesService.getStories.and.returnValue(of(mockStories));
    actions$ = of(loadStories({ pageSize: 10, offset: 0 }));

    effects.loadStories$.subscribe(action => {
      expect(action).toEqual(loadStoriesSuccess({ stories: mockStories }));
      done();
    });
  });

  it('should dispatch loadStoriesFailure on error', (done) => {
    storiesService.getStories.and.returnValue(throwError(() => new Error('API Error')));
    actions$ = of(loadStories({ pageSize: 10, offset: 0 }));

    effects.loadStories$.subscribe(action => {
      expect(action.type).toBe(loadStoriesFailure.type);
      done();
    });
  });
});