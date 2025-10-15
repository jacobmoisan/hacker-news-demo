import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Story } from './story.model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class StoriesService {
  private API_BASE_URL = 'http://localhost:5105';
  private latestStoriesUrl = `${this.API_BASE_URL}/stories/latest`;

  constructor(private http: HttpClient) {}

  getStories(pageSize: number, offset: number): Observable<Story[]> {
    let params = `?pageSize=${pageSize}&offset=${offset}`;
    return this.http.get<Story[]>(`${this.latestStoriesUrl}${params}`);
  }
}