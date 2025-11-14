import { Component, input, output } from '@angular/core';
import { Subject, debounceTime, distinctUntilChanged } from 'rxjs';

@Component({
  selector: 'app-search',
  imports: [],
  templateUrl: './search.html',
  styleUrl: './search.scss'
})
export class Search {
  placeholder = input('Search');
  inputClass = input<string>();
  debounceTime = input(300);

  onSearch = output<string>();

  private _searchSubject = new Subject<string>();

  constructor() {
    this._searchSubject.pipe(
      debounceTime(this.debounceTime()),
      distinctUntilChanged()
    ).subscribe(_ => this.onSearch.emit(_));
  }

  search = (event: Event) => this._searchSubject.next((event.target as HTMLInputElement).value);
}
