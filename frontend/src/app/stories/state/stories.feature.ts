import { createFeature, createReducer, on, createSelector } from '@ngrx/store';
import { createAction, props } from '@ngrx/store';
import { Story } from '../story.model';


export const loadStories = createAction(
  '[Stories] Load Stories',
  props<{ pageSize: number; offset: number; }>()
);

export const loadStoriesSuccess = createAction(
  '[Stories] Load Stories Success',
  props<{ stories: Story[] }>()
);

export const loadStoriesFailure = createAction(
  '[Stories] Load Stories Failure',
  props<{ error: any }>()
);


export interface StoriesState {
  stories: Story[];
  loading: boolean;
  error: any;
}

const initialState: StoriesState = {
  stories: [],
  loading: false,
  error: null,
};


const storiesReducer = createReducer(
  initialState,
  on(loadStories, (state) => ({
    ...state,
    loading: true,
    error: null
  })),
  on(loadStoriesSuccess, (state, { stories }) => ({
    ...state,
    stories,
    loading: false,
    error: null
  })),
  on(loadStoriesFailure, (state, { error }) => ({
    ...state,
    loading: false,
    error
  }))
);


export const storiesFeature = createFeature({
  name: 'stories',
  reducer: storiesReducer
});

export const selectStories = createSelector(
  storiesFeature.selectStories,
  (stories) => stories
);

export const selectLoading = createSelector(
  storiesFeature.selectLoading,
  (loading) => loading
);
export const selectError = createSelector(
  storiesFeature.selectError,
  (error) => error
);