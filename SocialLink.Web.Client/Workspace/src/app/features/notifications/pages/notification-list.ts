import { Component } from "@angular/core";
import { BaseComponent } from "../../../shared/base/base";
import { PageLoaderService } from "../../../core/services/page-loader.service";
import { ApiService } from "../../../core/services/api.service";
import { PagedResponse } from "../../../core/models/paged-response";
import { NotificationModel } from "../models/notification.model";
import { NotificationSearch } from "../../../core/models/search/notification-search";
import { finalize, take } from "rxjs";
import { Navigation } from "../../../shared/components/navigation/navigation";
import { AuthService } from "../../../core/services/auth.service";
import { Notification } from "./notification";

@Component({
  selector: 'app-notification-list',
  imports: [Notification, Navigation],
  template: `
        <app-navigation />

        <div class="notif-page">
          <div class="notif-page-header">
            <h2><i class="fa-regular fa-bell"></i> Notifications</h2>
            <p>Stay updated on what's happening around you.</p>
          </div>

          <div class="d-flex flex-column gap-2">
            @for (notification of notifications; track $index)
            {
              <app-notification [notification]="notification" (removed)="remove($event)"/>
            }
            @if (notifications.length == 0)
            {
              <div class="notif-empty">
                <div class="empty-icon"><i class="fa-regular fa-bell-slash"></i></div>
                <div class="empty-title">You're all caught up</div>
                <div class="empty-subtitle">There are no new notifications right now.</div>
              </div>
            }
          </div>
        </div>
    `,
  styles: `
    @import '../../../../assets/styles/variables.scss';

    .notif-page {
      max-width: 760px;
      margin: 0 auto;
      padding: 2rem 1rem 3rem;
    }
    .notif-page-header {
      margin-bottom: 1.25rem;
      animation: fade-in-up $dur-slow $ease-out both;

      h2 {
        font-size: 1.45rem;
        font-weight: 700;
        color: $ink;
        display: flex;
        align-items: center;
        gap: 0.6rem;
        margin: 0;

        i { color: $primary; }
      }
      p {
        margin: 0.25rem 0 0;
        color: $muted;
        font-size: 0.95rem;
      }
    }
    .notif-empty {
      text-align: center;
      padding: 3.5rem 1rem;
      color: $muted;

      .empty-icon {
        width: 76px;
        height: 76px;
        margin: 0 auto 0.75rem;
        border-radius: 50%;
        background: $primary-soft;
        color: $primary;
        display: flex;
        align-items: center;
        justify-content: center;
        font-size: 1.8rem;
      }
      .empty-title {
        font-size: 1.15rem;
        font-weight: 700;
        color: $ink;
      }
      .empty-subtitle {
        font-size: 0.92rem;
      }
    }
  `
})
export class NotificationList extends BaseComponent {
  searchOptions = new NotificationSearch();
  paging?: PagedResponse<NotificationModel>;
  notifications: NotificationModel[] = [];

  constructor(
    loaderService: PageLoaderService,
    private apiService: ApiService,
    private authService: AuthService
  ) {
    super(loaderService);

    this.searchOptions.userId = this.authService.getUserId();
    this.load();
  }

  load(): void {
    this.loading = true;
    this.apiService.post<PagedResponse<NotificationModel>>('/Notification/Get', this.searchOptions)
      .pipe(
        take(1),
        finalize(() => this.loading = false)
      ).subscribe({
        next: response => {
          this.paging = response;
          response.items && this.notifications.push(...response.items);
        }
      })
  }

  next(): void {
    this.searchOptions.page++;
    this.load();
  }

  remove(id: string): void {
    this.notifications = this.notifications.filter(_ => _.id != id);
  }
}
