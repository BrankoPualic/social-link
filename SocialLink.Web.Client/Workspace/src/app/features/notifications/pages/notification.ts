import { Component, OnInit, ViewContainerRef, input, output, viewChild } from "@angular/core";
import { BaseComponentGeneric } from "../../../shared/base/base";
import { NotificationModel } from "../models/notification.model";
import { PageLoaderService } from "../../../core/services/page-loader.service";
import { ApiService } from "../../../core/services/api.service";
import { take } from "rxjs";
import { NotificationComponentMap } from "../components/_notification-component-map";
import { CommonModule } from "@angular/common";
import { TimeAgoPipe } from "../../../core/pipes/time-ago.pipe";

@Component({
  selector: 'app-notification',
  imports: [CommonModule, TimeAgoPipe],
  template: `<div class="d-flex flex-column main-container p-2 col-12" [ngClass]="{'text-black-50' : notification()?.isRead}">
      <h6>{{ notification()?.title }}</h6>
      <ng-template #container />
      <div class="text-black-50 small">{{ notification()?.createdOn | timeAgo }}</div>
  </div>`,
  styles: `
    @import '../../../../assets/styles/variables.scss';

    .main-container {
      border-radius: $border-radius;
      box-shadow: $box-shadow2;
      margin: 0.5rem;
    }
  `
})
export class Notification extends BaseComponentGeneric<NotificationModel> implements OnInit {
  notification = input<NotificationModel>();
  removed = output<string>();
  container = viewChild('container', { read: ViewContainerRef });

  constructor(
    loaderService: PageLoaderService,
    private apiService: ApiService
  ) {
    super(loaderService);
  }

  ngOnInit(): void {
    if (!this.notification())
      return;

    const component = NotificationComponentMap[this.notification()?.typeId!];
    if (component) {
      this.container()?.clear();
      const ref = this.container()?.createComponent(component);
      ref?.setInput('details', this.notification()?.details);
      ref?.instance.readNotification.subscribe(() => this.read());
    }
  }

  read(): void {
    this.apiService.post('/notifications/read', this.notification())
      .pipe(take(1))
      .subscribe({
        next: () => this.removed.emit(this.notification()!.id!)
      })
  }
}
