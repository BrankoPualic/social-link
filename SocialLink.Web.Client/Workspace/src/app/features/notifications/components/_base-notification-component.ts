import { Directive, OnInit, input, output } from "@angular/core";
import { PageLoaderService } from "../../../core/services/page-loader.service";
import { BaseComponentGeneric } from "../../../shared/base/base";

@Directive()
export abstract class BaseNotificationComponent<T extends object> extends BaseComponentGeneric<T> implements OnInit {
  details = input<string>();
  data?: T;
  readNotification = output<void>();
  constructor(
    loaderService: PageLoaderService
  ) {
    super(loaderService);
  }

  ngOnInit(): void {
    if (this.details())
      this.data = JSON.parse(this.details()!);
  }

  markAsRead = () => this.readNotification.emit();
}
