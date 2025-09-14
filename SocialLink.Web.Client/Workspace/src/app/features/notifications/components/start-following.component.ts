import { Component } from "@angular/core";
import { BaseNotificationComponent } from "./_base-notification-component";
import { StartFollowingDetails } from "../models/details/start-following-details";
import { PageLoaderService } from "../../../core/services/page-loader.service";
import { RouterLink } from "@angular/router";

@Component({
  selector: 'app-start-following',
  imports: [RouterLink],
  template: `<div class="d-flex flex-column col-12">
    <div>{{ data?.Title }}</div>
    <div>{{ data?.Message }}</div>
    <a [routerLink]="['/profile', data?.FollowerId]">See profile</a>
    <div class="my-2 d-flex flex-row">
      <button class="btn btn-sm btn-outline-primary" (click)="markAsRead()">Mark as read</button>
    </div>
  </div>`
})
export class StartFollowingComponent extends BaseNotificationComponent<StartFollowingDetails>{
  constructor(
    loaderService: PageLoaderService
  ) {
    super(loaderService);
  }
}
