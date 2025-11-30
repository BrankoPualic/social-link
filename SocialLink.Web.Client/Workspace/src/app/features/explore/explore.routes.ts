import { Route } from "@angular/router";
import { authGuard } from "../../core/guards/auth.guard";

export const exploreRoutes: Route[] = [
  {
    path: "explore",
    title: "Explore",
    loadComponent: () => import("./pages/explore/explore").then(_ => _.Explore),
    canActivate: [authGuard]
  }
];
