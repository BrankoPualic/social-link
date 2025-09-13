import { Route } from "@angular/router";
import { authGuard } from "../../core/guards/auth.guard";

export const userRoutes: Route[] = [
  {
    path: "profile/:id",
    title: "Profile",
    loadComponent: () => import("./pages/profile/profile").then(_ => _.Profile),
    canActivate: [authGuard]
  }
];
