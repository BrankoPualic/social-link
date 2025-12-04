import { Component, OnInit, signal } from '@angular/core';
import { ActivatedRoute, NavigationEnd, Router, RouterLink, RouterOutlet } from '@angular/router';
import { filter, finalize, take } from 'rxjs';
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
import { PageLoaderComponent } from '../../../../shared/components/page-loader';

@Component({
  selector: 'app-inbox',
  imports: [Navigation, Search, TimeAgoPipe, RouterLink, RouterOutlet, Users, PageLoaderComponent],
  templateUrl: './inbox.html',
  styleUrl: './inbox.scss'
})
export class Inbox extends BaseComponent implements OnInit {
  conversations?: PagedResponse<ConversationModel>;
  searching = false;
  currentUserId?: string;

  inboxLoading = false;

  keyword = signal('');

  isBigScreen = true;
  isConversationSelected = false;

  constructor(
    loaderService: PageLoaderService,
    private apiService: ApiService,
    private authService: AuthService,
    public presenceService: PresenceService,
    public sharedService: SharedService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    super(loaderService);

    this.currentUserId = this.authService.getUserId();

    if (window.screen.width < 768)
      this.isBigScreen = false;

    this.isConversationSelected = false;
  }

  ngOnInit(): void {
    this.inboxLoading = true;
    this.apiService.post<PagedResponse<ConversationModel>>('/Inbox/Get', { userId: this.currentUserId }).pipe(
      take(1),
      finalize(() => this.inboxLoading = false)
    ).subscribe({
      next: response => this.conversations = response,
      error: _ => console.error(_)
    });

    this.updateIsConversationSelected();

    // 2. Handle navigation inside Angular app (including Back button)
    this.router.events
      .pipe(filter(event => event instanceof NavigationEnd))
      .subscribe(() => this.updateIsConversationSelected());
  }

  private updateIsConversationSelected() {
    const id = this.route.snapshot.firstChild?.paramMap.get('id');
    this.isConversationSelected = !!id;
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

  gotoConversation(id?: string): void {
    this.isConversationSelected = true;
    this.router.navigateByUrl(`/inbox/${id}`);
  }
}
