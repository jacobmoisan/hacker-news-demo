import { TestBed } from '@angular/core/testing';
import { provideMockStore } from '@ngrx/store/testing';
import { App } from './app';
import { selectStories, selectLoading } from './stories/state/stories.feature';
import { Story } from './stories/story.model';

describe('App', () => {
  const initialStories: Story[] = [];

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [App],
      providers: [
        provideMockStore({
          selectors: [
            { selector: selectStories, value: initialStories },
            { selector: selectLoading, value: false },
          ]
        })
      ]
    }).compileComponents();
  });

  it('should create the app', () => {
    const fixture = TestBed.createComponent(App);
    const app = fixture.componentInstance;
    expect(app).toBeTruthy();
  });

  it('should render toolbar title', () => {
    const fixture = TestBed.createComponent(App);
    fixture.detectChanges();
    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.querySelector('mat-toolbar span')?.textContent).toContain('Hacker News Stories');
  });
});
