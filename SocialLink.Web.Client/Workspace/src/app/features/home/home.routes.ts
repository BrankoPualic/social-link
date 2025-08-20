import { Routes } from "@angular/router";

export const homeRoutes: Routes = [
  {
    path: "home",
    title: "Home",
    loadComponent: () => import("./pages/home/home").then(_ => _.Home)
  }
];
