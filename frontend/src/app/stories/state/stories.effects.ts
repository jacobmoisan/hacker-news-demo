import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { StoriesService } from '../stories.service';
import { loadStories, loadStoriesSuccess, loadStoriesFailure } from './stories.feature';
import { switchMap, map, catchError } from 'rxjs/operators';
import { of } from 'rxjs';

@Injectable()
export class StoriesEffects {
  loadStories$ = createEffect(() =>
    this.actions$.pipe(
      ofType(loadStories),
      switchMap(action =>
        this.storiesService.getStories(action.pageSize, action.offset).pipe(
          map(stories => loadStoriesSuccess({ stories })),
          catchError(error => of(loadStoriesFailure({ error })))
        )
      )
    )
  );

  constructor(
    private actions$: Actions,
    private storiesService: StoriesService
  ) {}
}