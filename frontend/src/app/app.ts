import { Component } from '@angular/core';
import { StoriesComponent } from './stories/stories.component';
import { MatToolbarModule } from '@angular/material/toolbar';


@Component({
  selector: 'app-root',
  standalone: true,
  imports: [StoriesComponent, MatToolbarModule],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App { }