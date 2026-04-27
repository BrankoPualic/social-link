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
  template: `<div class="notif-card anim-rise" [class.is-read]="notification()?.isRead">
      <div class="notif-icon"><i class="fa-regular fa-bell"></i></div>
      <div class="notif-body">
        <div class="notif-title">{{ notification()?.title }}</div>
        <ng-template #container />
        <div class="notif-time">{{ notification()?.createdOn | timeAgo }}</div>
      </div>
  </div>`,
  styles: `
    @import '../../../../assets/styles/variables.scss';

    .notif-card {
      display: flex;
      gap: 0.85rem;
      align-items: flex-start;
      background: $surface;
      border: 1px solid $border;
      border-radius: $radius-md;
      padding: 0.9rem 1rem;
      box-shadow: $shadow-xs;
      position: relative;
      overflow: hidden;
      transition: box-shadow $dur $ease-out, transform $dur $ease-out, border-color $dur $ease-out;

      &::before {
        content: '';
        position: absolute;
        left: 0; top: 0; bottom: 0;
        width: 3px;
        background: $gradient-primary;
        transition: opacity $dur $ease-out;
      }

      &:hover {
        box-shadow: $shadow-sm;
        border-color: $border-strong;
        transform: translateY(-1px);
      }

      &.is-read {
        background: $surface-2;
        opacity: 0.78;
        &::before { opacity: 0; }

        .notif-title { color: $muted; }
      }
    }

    .notif-icon {
      width: 38px;
      height: 38px;
      border-radius: 50%;
      background: $primary-soft;
      color: $primary;
      display: flex;
      align-items: center;
      justify-content: center;
      flex-shrink: 0;
    }

    .notif-body { flex-grow: 1; min-width: 0; }

    .notif-title {
      font-weight: 600;
      color: $ink;
      font-size: 0.98rem;
      margin-bottom: 0.15rem;
    }

    .notif-time {
      color: $muted-2;
      font-size: 0.8rem;
      margin-top: 0.35rem;
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
    if (!this.notification()) return;

    const component = NotificationComponentMap[this.notification()?.typeId!];
    if (component) {
      this.container()?.clear();
      const ref = this.container()?.createComponent(component);
      ref?.setInput('details', this.notification()?.details);
      ref?.instance.readNotification.subscribe(() => this.read());
    }
  }

  read(): void {
    this.apiService.post('/Notification/Read', this.notification())
      .pipe(take(1))
      .subscribe({
        next: () => this.removed.emit(this.notification()!.id!)
      })
  }
}
