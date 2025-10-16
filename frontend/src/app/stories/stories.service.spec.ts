import { TestBed } from '@angular/core/testing';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { provideHttpClient } from '@angular/common/http';
import { StoriesService } from './stories.service';
import { HttpClient } from '@angular/common/http';
import { Story } from './story.model';

describe('StoriesService', () => {
  let service: StoriesService;
  let httpClient: HttpClient;
  let httpTestingController: HttpTestingController;

  beforeEach(() => {
      TestBed.configureTestingModule({
        providers: [
          provideHttpClient(),
          provideHttpClientTesting(),
          StoriesService
        ],
      });

      httpClient = TestBed.inject(HttpClient);
      httpTestingController = TestBed.inject(HttpTestingController);
      service = TestBed.inject(StoriesService);
    });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should get stories with correct params', () => {
    const mockStories: Story[] = [{ id: 1, title: 'title', url: 'url' }];

    service.getStories(10, 0).subscribe(stories => {
      expect(stories).toEqual(mockStories);
    });

    const req = httpTestingController.expectOne('/stories/latest?pageSize=10&offset=0');
    expect(req.request.method).toBe('GET');
    req.flush(mockStories);
  });
});
