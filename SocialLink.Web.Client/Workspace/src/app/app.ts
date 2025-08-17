import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { PageLoaderComponent } from './shared/components/page-loader';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, PageLoaderComponent],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
}
