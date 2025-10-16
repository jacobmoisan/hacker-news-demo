import { Injectable, inject } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { switchMap, map, catchError } from 'rxjs/operators';
import { of } from 'rxjs';
import { StoriesService } from '../stories.service';
import { loadStories, loadStoriesSuccess, loadStoriesFailure } from './stories.feature';

@Injectable()
export class StoriesEffects {
  // Inject directly in the effect factory to avoid referencing `this` during class field init
  loadStories$ = createEffect(
    (actions$ = inject(Actions), storiesService = inject(StoriesService)) =>
      actions$.pipe(
        ofType(loadStories),
        switchMap(action =>
          storiesService.getStories(action.pageSize, action.offset).pipe(
            map(stories => loadStoriesSuccess({ stories })),
            catchError(error => of(loadStoriesFailure({ error })))
          )
        )
      )
  );
}