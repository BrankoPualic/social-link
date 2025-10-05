import { Route } from "@angular/router";
import { authGuard } from "../../core/guards/auth.guard";

export const postRoutes: Route[] = [
  {
    path: "post/create",
    title: "Create Post",
    loadComponent: () => import("./pages/create-post/create-post").then(_ => _.CreatePost),
    canActivate: [authGuard]
  },
  {
    path: "post/:id",
    title: "Post",
    loadComponent: () => import("./pages/post/post").then(_ => _.Post),
    canActivate: [authGuard]
  }
];
