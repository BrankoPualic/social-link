import { Component, OnInit, signal } from '@angular/core';
import { Router, RouterLink, RouterOutlet } from '@angular/router';
import { finalize, take } from 'rxjs';
import { PagedResponse } from '../../../../core/models/paged-response';
import { TimeAgoPipe } from '../../../../core/pipes/time-ago.pipe';
import { ApiService } from '../../../../core/services/api.service';
import { AuthService } from '../../../../core/services/auth.service';
import { PageLoaderService } from '../../../../core/services/page-loader.service';
import { SharedService } from '../../../../core/services/shared.service';
import { BaseComponent } from '../../../../shared/base/base';
import { Navigation } from '../../../../shared/components/navigation/navigation';
import { Search } from '../../../../shared/components/search/search';
import { Users } from '../../../user/components/users/users';
import { UserLightModel } from '../../../user/models/user-light.model';
import { ConversationModel } from '../../models/conversation.model';
import { PresenceService } from '../../services/presence.service';

@Component({
  selector: 'app-inbox',
  imports: [Navigation, Search, TimeAgoPipe, RouterLink, RouterOutlet, Users],
  templateUrl: './inbox.html',
  styleUrl: './inbox.scss'
})
export class Inbox extends BaseComponent implements OnInit {
  conversations?: PagedResponse<ConversationModel>;
  //users?: PagedResponse<UserLightModel>;
  searching = false;
  currentUserId?: string;

  keyword = signal('');

  constructor(
    loaderService: PageLoaderService,
    private apiService: ApiService,
    private authService: AuthService,
    public presenceService: PresenceService,
    public sharedService: SharedService,
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
    if (!value) {
      this.searching = false;
      return;
    }

    this.searching = true;
    this.keyword.set(value);
  }

  openConversation(user: UserLightModel): void {
    this.loading = true;
    this.apiService.post<string>('/Inbox/CreateConversation', { users: [user.id, this.currentUserId] }).pipe(
      take(1),
      finalize(() => this.loading = false)
    ).subscribe({
      next: conversationId => this.router.navigateByUrl(`/inbox/${conversationId}`),
    });
  }
}
