import { Route } from "@angular/router";
import { authGuard } from "../../core/guards/auth.guard";

export const messagingRoutes: Route[] = [
  {
    path: "inbox",
    title: "Inbox",
    loadComponent: () => import("./pages/inbox/inbox").then(_ => _.Inbox),
    canActivate: [authGuard],
  },
  {
    path: "inbox/:id",
    title: "Conversation",
    loadComponent: () => import("./pages/conversation/conversation").then(_ => _.Conversation),
    canActivate: [authGuard]
  }
];
