import { Routes } from "@angular/router";

export const errorRoutes: Routes = [
  {
    path: "error/unauthorized",
    title: "Unauthorized",
    loadComponent: () => import("./pages/unauthorized").then(_ => _.Unauthorized)
  },
  {
    path: "error/not-found",
    title: "Not Found",
    loadComponent: () => import("./pages/not-found").then(_ => _.NotFound)
  },
];
