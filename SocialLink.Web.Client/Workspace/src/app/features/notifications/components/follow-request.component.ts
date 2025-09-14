import { Component } from "@angular/core";
import { PageLoaderService } from "../../../core/services/page-loader.service";
import { BaseNotificationComponent } from "./_base-notification-component";
import { FollowRequestDetails } from "../models/details/follow-request-details";
import { RouterLink } from "@angular/router";
import { eNotificationActionMethodType } from "../../../core/enumerators/notification-action-method-type.enum";
import { ApiService } from "../../../core/services/api.service";
import { finalize, take } from "rxjs";

@Component({
  selector: 'app-follow-request',
  imports: [RouterLink],
  template: `<div class="d-flex flex-column col-12">
    <div>{{ data?.Title }}</div>
    <div>{{ data?.Message }}</div>
    <div class="my-2 d-flex flex-md-row flex-column">
      @for (action of data?.Actions; track $index)
      {
          <button class="btn btn-sm btn-outline-primary px-3 me-1 mt-1 mt-md-0" (click)="apiAction(action.Method, action.Endpoint)">{{ action.Label }}</button>
      }
    </div>

    <a [routerLink]="['/profile', data?.FollowerId]">See profile</a>
  </div>`
})
export class FollowRequestComponent extends BaseNotificationComponent<FollowRequestDetails> {
  constructor(
    loaderService: PageLoaderService,
    private apiService: ApiService
  ) {
    super(loaderService);
  }

  // TODO: find a way to create generic method
  apiAction(method?: eNotificationActionMethodType, endpoint?: string) {
    this.loading = true;
    this.apiService.post(endpoint!, {
        followerId: this.data?.FollowerId,
        followingId: this.data?.UserId
    }).pipe(
      take(1),
      finalize(() => this.loading = false)
    ).subscribe({
      next: () => this.markAsRead()
    })
  }
}
