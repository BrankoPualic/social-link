import { Injectable, signal } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class PageLoaderService {
  // TODO: Investigate if it would be better to inject component into other components and handle state by simple loading flag inside base component?
  state = signal(false);

  show = () => this.state.set(true);
  hide = () => this.state.set(false);
}
