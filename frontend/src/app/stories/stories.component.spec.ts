import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideMockStore, MockStore } from '@ngrx/store/testing';

import { StoriesComponent } from './stories.component';
import { selectStories, selectLoading } from './state/stories.feature';
import { Story } from './story.model';

describe('StoriesComponent', () => {
  let component: StoriesComponent;
  let fixture: ComponentFixture<StoriesComponent>;
  let store: MockStore;

  const initialStories: Story[] = [
    { id: 1, title: 'title1', url: 'url1' },
    { id: 2, title: 'title2', url: 'url2' }
  ];

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [StoriesComponent],
      providers: [
        provideMockStore({
          selectors: [
            { selector: selectStories, value: initialStories },
            { selector: selectLoading, value: false },
          ]
        })
      ]
    })
    .compileComponents();

    store = TestBed.inject(MockStore);
    fixture = TestBed.createComponent(StoriesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should filter stories based on search term', () => {
    component.searchTerm = 'title';
    const filtered = component.filteredStories;
    expect(filtered.length).toBe(2);
    expect(filtered[0].title).toBe('title1');
  });
});
