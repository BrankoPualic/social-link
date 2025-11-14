import { Component, OnInit } from '@angular/core';
import { finalize, take } from 'rxjs';
import { PagedResponse } from '../../../../core/models/paged-response';
import { TimeAgoPipe } from '../../../../core/pipes/time-ago.pipe';
import { ApiService } from '../../../../core/services/api.service';
import { AuthService } from '../../../../core/services/auth.service';
import { Navigation } from '../../../../shared/components/navigation/navigation';
import { Search } from '../../../../shared/components/search/search';
import { UserLightModel } from '../../../user/models/user-light.model';
import { ConversationModel } from '../../models/conversation.model';
import { PresenceService } from '../../services/presence.service';
import { BaseComponent } from '../../../../shared/base/base';
import { PageLoaderService } from '../../../../core/services/page-loader.service';
import { Router, RouterLink } from '@angular/router';

@Component({
  selector: 'app-inbox',
  imports: [Navigation, Search, TimeAgoPipe, RouterLink],
  templateUrl: './inbox.html',
  styleUrl: './inbox.scss'
})
export class Inbox extends BaseComponent implements OnInit {
  conversations?: PagedResponse<ConversationModel>;
  users?: PagedResponse<UserLightModel>;
  searching = false;
  currentUserId?: string;

  constructor(
    loaderService: PageLoaderService,
    private apiService: ApiService,
    private authService: AuthService,
    public presenceService: PresenceService,
    private router: Router
  ) {
    super(loaderService);

    this.currentUserId = this.authService.getUserId();
  }

  ngOnInit(): void {
    this.apiService.post<PagedResponse<ConversationModel>>('/Inbox/Get', { userId: this.currentUserId }).pipe(
      take(1)
    ).subscribe({
      next: response => this.conversations = response,
      error: _ => console.error(_)
    });
  }

  onSearch(value: string) {
    this.users = undefined;

    if (!value) {
      this.searching = false;
      return;
    }

    this.searching = true;
    this.apiService.post<PagedResponse<UserLightModel>>('/User/SearchContracts', { keyword: value, following: true }).pipe(
      take(1)
    ).subscribe({
      next: response => this.users = response
    });
  }

  getImage(url?: string): string {
    // TODO: Create separate shared service or a global function to handle this default state
    // Also see reference inside profile.ts
    // Create custom unisex profile avatar image. See linkedin for reference.
    return url || './assets/images/man.png';
  }

  enterConversation(userId: string): void {
    this.loading = true;
    this.apiService.post<string>('/Inbox/CreateConversation', { users: [userId, this.currentUserId] }).pipe(
      take(1),
      finalize(() => this.loading = false)
    ).subscribe({
      next: conversationId => this.router.navigateByUrl(`/inbox/${conversationId}`),
    });
  }
}
