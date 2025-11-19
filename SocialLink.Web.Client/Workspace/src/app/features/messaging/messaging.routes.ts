import { Route } from "@angular/router";
import { authGuard } from "../../core/guards/auth.guard";

export const messagingRoutes: Route[] = [
  {
    path: "inbox",
    title: "Inbox",
    loadComponent: () => import("./pages/inbox/inbox").then(_ => _.Inbox),
    canActivate: [authGuard],
    children: [
      {
        path: ":id",
        title: "Conversation",
        loadComponent: () => import("./pages/conversation/conversation").then(_ => _.Conversation),
      }
    ]
  },
  {
    path: "create-group",
    title: "Create Group",
    loadComponent: () => import("./pages/create-group/create-group").then(_ => _.CreateGroup),
    canActivate: [authGuard]
  }
];
