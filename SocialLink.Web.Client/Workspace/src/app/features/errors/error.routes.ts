import { Routes } from "@angular/router";

export const errorRoutes: Routes = [
  {
    path: "unauthorized",
    title: "Unauthorized",
    loadComponent: () => import("./pages/unauthorized").then(_ => _.Unauthorized)
  },
  {
    path: "not-found",
    title: "Not Found",
    loadComponent: () => import("./pages/not-found").then(_ => _.NotFound)
  },
];
