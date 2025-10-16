import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Story } from './story.model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class StoriesService {
  private latestStoriesUrl = `https://dhhxqk9v7k.execute-api.us-east-2.amazonaws.com/stories/latest`;

  constructor(private http: HttpClient) {}

  getStories(pageSize: number, offset: number): Observable<Story[]> {
    let params = `?pageSize=${pageSize}&offset=${offset}`;
    return this.http.get<Story[]>(`${this.latestStoriesUrl}${params}`);
  }
}