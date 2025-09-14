import { Route } from "@angular/router";
import { authGuard } from "../../core/guards/auth.guard";

export const notificationRoutes: Route[] = [
  {
    path: "notifications",
    title: "Notifications",
    loadComponent: () => import("./pages/notification-list").then(_ => _.NotificationList),
    canActivate: [authGuard]
  }
];
