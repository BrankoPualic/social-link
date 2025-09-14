import { Component } from "@angular/core";
import { BaseComponent } from "../../../shared/base/base";
import { PageLoaderService } from "../../../core/services/page-loader.service";
import { ApiService } from "../../../core/services/api.service";
import { PagedResponse } from "../../../core/models/paged-response";
import { NotificationModel } from "../models/notification.model";
import { NotificationSearch } from "../../../core/models/search/notification-search";
import { finalize, take } from "rxjs";
import { Navigation } from "../../../shared/components/navigation";
import { AuthService } from "../../../core/services/auth.service";
import { Notification } from "./notification";

@Component({
  selector: 'app-notification-list',
  imports: [Notification, Navigation],
  template: `<div class="d-flex flex-row">
        <app-navigation />

        <div class="container-fluid p-4">
          <div class="d-flex flex-column">
            @for (notification of notifications; track $index)
            {
              <app-notification [notification]="notification" (removed)="remove($event)"/>
            }
            @if (notifications.length == 0)
            {
              <span>There are no new notifications.</span>
            }
          </div>
        </div>
  </div>`
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
    this.apiService.post<PagedResponse<NotificationModel>>('/notifications', this.searchOptions)
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
