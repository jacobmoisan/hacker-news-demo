import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatListModule } from '@angular/material/list';
import { MatPaginatorModule } from '@angular/material/paginator';
import { Story } from './story.model';
import { Store } from '@ngrx/store';
import { loadStories, selectStories, selectLoading } from './state/stories.feature';

@Component({
  selector: 'hacker-news-stories',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatInputModule,
    MatFormFieldModule,
    MatListModule,
    MatButtonModule,
    MatPaginatorModule,
    MatCardModule,
  ],
  templateUrl: './stories.component.html',
  styleUrls: ['./stories.component.css']
})
export class StoriesComponent {
  private store = inject<Store>(Store);

  stories = this.store.selectSignal(selectStories);
  loading = this.store.selectSignal(selectLoading);

  searchTerm = '';
  pageSize = 10;
  pageIndex = 0;

  constructor() {
    this.loadStories();
  }

  loadStories() {
    this.store.dispatch(loadStories({
      pageSize: this.pageSize,
      offset: this.pageIndex * this.pageSize
    }));
  }

  get filteredStories() {
    if (!this.searchTerm) return this.stories();
    return this.stories().filter((s: Story) =>
      s.title.toLowerCase().includes(this.searchTerm.toLowerCase())
    );
  }

  onPageChange(event: any) {
    this.pageIndex = event.pageIndex;
    this.pageSize = event.pageSize;
    this.loadStories();
  }
}