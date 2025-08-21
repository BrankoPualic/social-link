import { Routes } from "@angular/router";
import { authGuard } from "../../core/guards/auth.guard";

export const homeRoutes: Routes = [
  {
    path: "",
    title: "Home",
    loadComponent: () => import("./pages/home/home").then(_ => _.Home),
    pathMatch: "full",
    canActivate: [authGuard]
  }
];
