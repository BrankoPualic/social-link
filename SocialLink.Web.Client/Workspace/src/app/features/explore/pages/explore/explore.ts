import { Component, signal } from '@angular/core';
import { Navigation } from '../../../../shared/components/navigation/navigation';
import { Posts } from '../../../post/components/posts/posts';
import { Search } from '../../../../shared/components/search/search';
import { Users } from '../../../user/components/users/users';
import { UserLightModel } from '../../../user/models/user-light.model';
import { Router } from '@angular/router';

@Component({
  selector: 'app-explore',
  imports: [Navigation, Posts, Search, Users],
  templateUrl: './explore.html',
  styleUrl: './explore.scss'
})
export class Explore {
  keyword = signal('');
  isSearching = false;

  constructor(
    private router: Router
  ) { }

  onSearch(value: string) {
    if (!value) {
      this.isSearching = false;
      return;
    }

    this.isSearching = true;
    this.keyword.set(value);
  }

  goToProfile(user: UserLightModel): void {
    this.router.navigateByUrl(`/profile/${user.id}`);
  }
}
