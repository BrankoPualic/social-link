import { Injectable, signal } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class PageLoaderService {
  state = signal(false);

  show = () => this.state.set(true);
  hide = () => this.state.set(false);
}
