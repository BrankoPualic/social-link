import { Component } from "@angular/core";
import { BaseNotificationComponent } from "./_base-notification-component";

@Component({
    selector: 'app-default-notification-request',
    template: `<div>
    <a href="#" (click)="markAsRead(); $event.preventDefault()"></a>
  </div>`
})
export class DefaultComponent extends BaseNotificationComponent<{}>{ }
