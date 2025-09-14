import { Route } from "@angular/router";
import { authGuard } from "../../core/guards/auth.guard";
import { ownerGuard } from "../../core/guards/owner.guard";

export const userRoutes: Route[] = [
  {
    path: "profile/:id",
    title: "Profile",
    loadComponent: () => import("./pages/profile/profile").then(_ => _.Profile),
    canActivate: [authGuard]
  },
  {
    path: "profile/:id/edit",
    title: "Edit Profile",
    loadComponent: () => import("./pages/edit-profile/edit-profile").then(_ => _.EditProfile),
    canActivate: [authGuard, ownerGuard]
  }
];
