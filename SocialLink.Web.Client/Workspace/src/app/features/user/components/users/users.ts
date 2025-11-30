import { Component, OnChanges, OnInit, SimpleChanges, input, output } from '@angular/core';
import { SharedService } from '../../../../core/services/shared.service';
import { PresenceService } from '../../../messaging/services/presence.service';
import { PageLoaderComponent } from '../../../../shared/components/page-loader';
import { ApiService } from '../../../../core/services/api.service';
import { PagedResponse } from '../../../../core/models/paged-response';
import { UserLightModel } from '../../models/user-light.model';
import { finalize, take } from 'rxjs';

@Component({
  selector: 'app-users',
  imports: [PageLoaderComponent],
  templateUrl: './users.html',
  styleUrl: './users.scss'
})
export class Users implements OnChanges {
  showPresence = input(false);
  onlyFollowing = input(false);
  keyword = input('');
  onClick = output<UserLightModel>();
  usersLoading = false;
  users?: PagedResponse<UserLightModel>;

  constructor(
    public sharedService: SharedService,
    public presenceService: PresenceService,
    private apiService: ApiService
  ) { }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['keyword'])
      this.load();
  }

  load(): void {
    this.usersLoading = true;

    this.apiService.post<PagedResponse<UserLightModel>>('/User/SearchContracts', { keyword: this.keyword(), following: this.onlyFollowing() }).pipe(
      take(1),
      finalize(() => this.usersLoading = false)
    )
      .subscribe({
        next: result => this.users = result
      })
  }

  emitOnClick(user: UserLightModel): void {
    this.onClick.emit(user);
  }
}
